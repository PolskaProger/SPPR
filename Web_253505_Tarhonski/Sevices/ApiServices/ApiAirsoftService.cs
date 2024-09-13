using System.Text.Json;
using System.Text;
using Web_253505_Tarhonski.Domain.Models;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Domain.Entities;

namespace Web_253505_Tarhonski.Sevices.ApiServices
{
    public class ApiAirsoftService : IAirsoftService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiAirsoftService> _logger;

        public ApiAirsoftService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiAirsoftService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAirsoftAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(int id)
        {
            throw new NotImplementedException();
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


        public Task UpdateAirsoftAsync(int id, Airsoft airsoft, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }

}
