using Web_253505_Tarhonski.Services.AirsoftService;
using Web_253505_Tarhonski.Services.CategoryService;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Extentions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IAirsoftService, MemoryAirsoftService>();
        }
    }
}
