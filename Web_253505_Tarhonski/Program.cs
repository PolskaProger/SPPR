using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski;
using Web_253505_Tarhonski.API.Data;
using Web_253505_Tarhonski.Extentions;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.ApiServices;
using Web_253505_Tarhonski.Sevices.CategoryService;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCustomServices();
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
{
    opt.BaseAddress = new Uri(UriData.ApiUri);
});
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
    {
        options.Conventions.AddAreaPageRoute("Admin", "/Index", "Admin/Index");
    });

builder.Services
.AddHttpClient<IAirsoftService, ApiAirsoftService>(opt =>
opt.BaseAddress = new Uri(UriData.ApiUri));



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

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
