using System.Text.Json;
using System.Text;
using Web_253505_Tarhonski.Domain.Models;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Sevices.FileService;
using Web_253505_Tarhonski.Sevices.TokenService;

namespace Web_253505_Tarhonski.Sevices.ApiServices
{
    public class ApiAirsoftService : IAirsoftService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly IFileService _fileService;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ITokenAccessor _tokenAccessor;
        private readonly ILogger<ApiAirsoftService> _logger;

        public ApiAirsoftService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiAirsoftService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            // Устанавливаем изображение по умолчанию, если файл не передан
            airsoft.ImagePath = "Images/noimage.jpg";

            // Проверяем наличие файла и сохраняем его с помощью FileService
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    airsoft.ImagePath = imageUrl;
                }
            }

            // Формируем URI для POST-запроса к контроллеру Airsoft
            var uri = new Uri(_httpClient.BaseAddress?.AbsoluteUri + "airsofts");

            try
            {
                // Отправляем POST-запрос с объектом Airsoft в формате JSON
                var response = await _httpClient.PostAsJsonAsync(uri, airsoft, _serializerOptions);

                // Проверяем, успешен ли запрос
                if (response.IsSuccessStatusCode)
                {
                    // Читаем данные из ответа (ResponseData<Airsoft>)
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<Airsoft>>(_serializerOptions);
                    return data;
                }

                // Если запрос не успешен, логируем ошибку и возвращаем ошибочный результат
                _logger.LogError($"-----> Ошибка создания Airsoft. Статус: {response.StatusCode}");
                return ResponseData<Airsoft>.Error($"Ошибка при создании объекта Airsoft. Код статуса: {response.StatusCode}");
            }
            catch (JsonException ex)
            {
                // Обрабатываем возможные исключения при сериализации/десериализации
                _logger.LogError($"-----> Ошибка сериализации: {ex.Message}");
                return ResponseData<Airsoft>.Error($"Ошибка сериализации данных: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Обрабатываем любые другие исключения (например, проблемы с соединением)
                _logger.LogError($"-----> Исключение при создании Airsoft: {ex.Message}");
                return ResponseData<Airsoft>.Error($"Ошибка при создании объекта Airsoft: {ex.Message}");
            }
        }






        // Удаление объекта Airsoft
        public async Task DeleteAirsoftAsync(Guid id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress.AbsoluteUri}airsofts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Ошибка при удалении объекта: {response.StatusCode}");
                throw new Exception("Ошибка при удалении объекта.");
            }
        }

        // Получение объекта по ID
        public async Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(Guid id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress.AbsoluteUri}airsofts/{id}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseData<Airsoft>>(_serializerOptions);
                    return result!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации JSON: {ex.Message}");
                    return ResponseData<Airsoft>.Error("Ошибка десериализации данных.");
                }
            }

            _logger.LogError($"Ошибка при получении объекта по ID: {response.StatusCode}");
            return ResponseData<Airsoft>.Error("Ошибка при получении объекта.");
        }

        // Обновление объекта Airsoft
        public async Task UpdateAirsoftAsync(Guid id, Airsoft updatedAirsoft, IFormFile? formFile)
        {

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            // Если загружен новый файл изображения
            if (formFile != null)
            {
                await _fileService.DeleteFileAsync(updatedAirsoft.ImagePath);
                // Сохраняем новое изображение с помощью FileService
                var newImageUrl = await _fileService.SaveFileAsync(formFile);

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    updatedAirsoft.ImagePath = newImageUrl;
                }
            }
            

            // Создаем запрос с обновлённым объектом Airsoft
            var content = new StringContent(JsonSerializer.Serialize(updatedAirsoft, _serializerOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_httpClient.BaseAddress.AbsoluteUri}airsofts/{id}", content);

            // Проверяем статус ответа
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Ошибка при обновлении объекта: {response.StatusCode}");
                throw new Exception("Ошибка при обновлении объекта.");
            }
        }




        public async Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Формируем QueryString для категории и номера страницы
            var queryParams = new Dictionary<string, string?>
            {
                { "category", categoryNormalizedName },
                { "page", pageNo.ToString() }
            };

            if (!_pageSize.Equals("3"))
            {
                queryParams.Add("pageSize", _pageSize);
            }

            var queryString = QueryString.Create(queryParams);

            var urlString = $"{_httpClient.BaseAddress.AbsoluteUri}airsofts{queryString}";
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.GetAsync(new Uri(urlString));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Airsoft>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации JSON: {ex.Message}");
                    return ResponseData<ListModel<Airsoft>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Данные не получены от сервера. Ошибка: {response.StatusCode}");
            return ResponseData<ListModel<Airsoft>>.Error($"Данные не получены от сервера. Ошибка: {response.StatusCode}");
        }
        public async Task<ResponseData<ListModel<Airsoft>>> GetAllAirsoftsAsync(string? categoryNormalizedName)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            // Формируем QueryString для категории (без параметров пагинации)
            var queryParams = new Dictionary<string, string?>
            {
                { "category", categoryNormalizedName }
            };

            var queryString = QueryString.Create(queryParams);
            var urlString = $"{_httpClient.BaseAddress.AbsoluteUri}airsofts{queryString}";

            var response = await _httpClient.GetAsync(new Uri(urlString));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Десериализация данных в список Airsoft без модели для пагинации
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Airsoft>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации JSON: {ex.Message}");
                    return ResponseData<ListModel<Airsoft>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Данные не получены от сервера. Ошибка: {response.StatusCode}");
            return ResponseData<ListModel<Airsoft>>.Error($"Данные не получены от сервера. Ошибка: {response.StatusCode}");
        }

    }

}
