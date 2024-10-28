using Web_253505_Tarhonski.HelperClasses;
using Web_253505_Tarhonski.Services.AirsoftService;
using Web_253505_Tarhonski.Services.CategoryService;
using Web_253505_Tarhonski.Services.TokenService;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.ApiServices;
using Web_253505_Tarhonski.Sevices.AuthService;
using Web_253505_Tarhonski.Sevices.CategoryService;
using Web_253505_Tarhonski.Sevices.FileService;
using Web_253505_Tarhonski.Sevices.TokenService;

namespace Web_253505_Tarhonski.Extentions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IAirsoftService, MemoryAirsoftService>();
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{UriData.ApiUri}Files"));
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddScoped<IAuthService, KeycloakAuthService>();
        }
    }
}
