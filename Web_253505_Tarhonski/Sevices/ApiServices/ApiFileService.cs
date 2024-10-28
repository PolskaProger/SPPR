using System.Text.Json;
using Web_253505_Tarhonski.Sevices.FileService;
using Web_253505_Tarhonski.Sevices.TokenService;

namespace Web_253505_Tarhonski.Sevices.ApiServices
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiFileService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, ILogger<ApiFileService> logger, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _tokenAccessor = tokenAccessor;
            _serializerOptions = new JsonSerializerOptions();
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            // Устанавливаем заголовок авторизации перед отправкой запроса
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            _logger.LogError($"-----> File not saved. Error: {response.StatusCode}");
            return string.Empty;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            // Устанавливаем заголовок авторизации перед отправкой запроса
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var uri = new Uri(_httpClient.BaseAddress?.AbsoluteUri + $"/{fileName.Split('/').Last()}");
            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> File not deleted. Error: {response.StatusCode}");
            }
        }
    }
}
