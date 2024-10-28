using System.Text.Json;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Sevices.ApiServices
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCategoryService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<ResponseData<ListModel<Category>>> GetCategoryListAsync()
        {
            var response = await _httpClient.GetAsync("categories");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Category>>>();
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка парсинга: {ex.Message}");
                    return ResponseData<ListModel<Category>>.Error($"Ошибка парсинга: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка получения данных: {response.StatusCode}");
            return ResponseData<ListModel<Category>>.Error($"Ошибка получения данных: {response.StatusCode}");
        }
        public async Task<ResponseData<Category>> GetCategoryByIdAsync(Guid? id)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress.AbsoluteUri}category/{id}");
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);
                return result!;
            }
            return ResponseData<Category>.Error("Ошибка при получении объекта.");
        }
    }
}
