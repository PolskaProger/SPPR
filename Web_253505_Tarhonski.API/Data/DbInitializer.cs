using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski.Domain.Entities;

namespace Web_253505_Tarhonski.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Создаем scope для получения сервисов
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // Выполнение миграции перед добавлением данных
            await context.Database.MigrateAsync();

            // Получение URL приложения из конфигурации
            var appUrl = app.Configuration["ApplicationUrl"] ?? "https://localhost:7002";

            // Проверяем, есть ли категории
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Автоматы", NormalizedName = "rifles" },
                    new Category { Name = "Пистолеты", NormalizedName = "pistols" },
                    new Category { Name = "Дробовики", NormalizedName = "shotguns" },
                    new Category { Name = "Пулемёты", NormalizedName = "machineguns" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Проверяем, есть ли объекты Airsoft
            if (!context.Airsofts.Any())
            {
                var riflesCategory = context.Categories.First(c => c.NormalizedName == "rifles");
                var pistolsCategory = context.Categories.First(c => c.NormalizedName == "pistols");
                var machinegunsCategory = context.Categories.First(c => c.NormalizedName == "machineguns");
                var shotgunsCategory = context.Categories.First(c => c.NormalizedName == "shotguns");

                var airsofts = new List<Airsoft>
                {
                    new Airsoft
                    {
                        Name = "CYMA AK-47",
                        Description = "Классический автомат для страйкбола",
                        Price = 150.00m,
                        ImagePath = $"{appUrl}/Images/AK-47.jpg",
                        Category = riflesCategory
                    },
                    new Airsoft
                    {
                        Name = "CYMA M4A1",
                        Description = "Штурмовая винтовка с малым весом",
                        Price = 200.00m,
                        ImagePath = $"{appUrl}/Images/M4A1.jpg",
                        Category = riflesCategory
                    },
                    new Airsoft
                    {
                        Name = "CYMA Glock 18С",
                        Description = "Пистолет с автоматическим режимом",
                        Price = 100.00m,
                        ImagePath = $"{appUrl}/Images/Glock-18C.jpg",
                        Category = pistolsCategory
                    },
                    new Airsoft
                    {
                        Name = "CYMA M60",
                        Description = "Тяжёлый пулемёт с великолепной скорострельностью",
                        Price = 300.00m,
                        ImagePath = $"{appUrl}/Images/m60.jpg",
                        Category = machinegunsCategory
                    },
                    new Airsoft
                    {
                        Name = "CYMA Remington M870",
                        Description = "Идеален для CQB",
                        Price = 75.00m,
                        ImagePath = $"{appUrl}/Images/m870.jpg",
                        Category = shotgunsCategory
                    },
                     new Airsoft
                    {
                        Name = "CYMA G36",
                        Description = "Немецкая классика в твоих руках",
                        Price = 125.00m,
                        ImagePath = $"{appUrl}/Images/G36.jpg",
                        Category = riflesCategory
                    },
                      new Airsoft
                    {
                        Name = "CYMA VSS",
                        Description = "Тихая и дальнобойная винтовка",
                        Price = 500.00m,
                        ImagePath = $"{appUrl}/Images/VSS.jpg",
                        Category = riflesCategory
                    }
                };

                context.Airsofts.AddRange(airsofts);
                await context.SaveChangesAsync();
            }
        }
    }
}

