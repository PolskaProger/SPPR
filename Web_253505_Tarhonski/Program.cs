using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using Web_253505_Tarhonski;
using Web_253505_Tarhonski.API.Data;
using Web_253505_Tarhonski.Extentions;
using Web_253505_Tarhonski.HelperClasses;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.ApiServices;
using Web_253505_Tarhonski.Sevices.CategoryService;
using Web_253505_Tarhonski.Sevices.FileService;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCustomServices();
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
{
    opt.BaseAddress = new Uri(UriData.ApiUri);
});
builder.Services
.AddHttpClient<IAirsoftService, ApiAirsoftService>(opt =>
opt.BaseAddress = new Uri(UriData.ApiUri));

builder.Services.AddDistributedMemoryCache(); // Использование памяти для сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true; // Без доступа из JavaScript
    options.Cookie.IsEssential = true; // Важность cookie для сессии
});

var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority =
    $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); // Customize scopes as needed
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; // позволяет обращаться клокальному Keycloak по http
    options.MetadataAddress =$"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages().RequireAuthorization();
app.UseRouting();

app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
