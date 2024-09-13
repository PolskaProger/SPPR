using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;
using Web_253505_Tarhonski.Services.CategoryService;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Services.AirsoftService
{
    public class MemoryAirsoftService : IAirsoftService
    {
        private List<Airsoft> _airsofts; // Список объектов Airsoft
        private List<Category> _categories; // Список категорий
        private int _itemsPerPage; // Количество элементов на страницу

        // Конструктор, внедряющий конфигурацию и CategoryService
        public MemoryAirsoftService(IConfiguration config, ICategoryService categoryService)
        {
            // Получаем значение из конфигурации
            _itemsPerPage = int.Parse(config["ItemsPerPage"] ?? "3");

            // Инициализация списка категорий через сервис категорий
            var categoryResponse = categoryService.GetCategoryListAsync().Result;
            _categories = categoryResponse.Data.Items;

            // Заполнение коллекции объектов Airsoft
            SetupData();
        }

        // Метод для получения списка с фильтрацией и пагинацией
        public Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            int pageSize = _itemsPerPage; // Получение размера страницы из конфигурации
            var filteredAirsofts = _airsofts.AsQueryable();

            // Фильтрация по категории
            if (!string.IsNullOrEmpty(categoryNormalizedName) && categoryNormalizedName != "Все")
            {
                filteredAirsofts = filteredAirsofts.Where(a => a.Category.NormalizedName == categoryNormalizedName);
            }

            // Пагинация
            var totalItems = filteredAirsofts.Count();
            var paginatedAirsofts = filteredAirsofts
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new ResponseData<ListModel<Airsoft>>
            {
                Data = new ListModel<Airsoft>
                {
                    Items = paginatedAirsofts,
                    TotalCount = totalItems,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                Successfull = true
            };

            return Task.FromResult(response);
        }



        /// <summary>
        /// Инициализация данных Airsoft
        /// </summary>
        private void SetupData()
        {
            _airsofts = new List<Airsoft>
            {
                new Airsoft
                {
                    ID = 1,
                    Name = "CYMA AK-47",
                    Description = "Классический автомат для страйкбола",
                    Price = 150.00m,
                    ImagePath = "AK-47.jpg",
                    MimeType = "image/jpeg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("rifles"))
                },
                new Airsoft
                {
                    ID = 2,
                    Name = "CYMA M4A1",
                    Description = "Штурмовая винтовка с малым весом",
                    Price = 200.00m,
                    ImagePath = "M4A1.jpg",
                    MimeType = "image/jpeg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("rifles"))
                },
                new Airsoft
                {
                    ID = 3,
                    Name = "CYMA Glock 18С",
                    Description = "Пистолет с автоматическим режимом",
                    Price = 100.00m,
                    ImagePath = "Glock-18C.jpg",
                    MimeType = "image/jpeg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("pistols"))
                },
                new Airsoft
                {
                    ID = 4,
                    Name = "CYMA M60",
                    Description = "Тяжёлый пулемёт с великолепной скорострельностью",
                    Price = 300.00m,
                    ImagePath = "m60.jpg",
                    MimeType = "image/jpeg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("machineguns"))
                },
            new Airsoft
                {
                    ID = 5,
                    Name = "CYMA Remington M870",
                    Description = "Идеален для CQB",
                    Price = 75.00m,
                    ImagePath = "m870.jpg",
                    MimeType = "image/jpeg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("shotguns"))
                }
                    
            };
        }

        // Остальные методы интерфейса IAirsoftService (нужно будет реализовать позднее)
        public Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(int id) => Task.FromResult(new ResponseData<Airsoft>());
        public Task UpdateAirsoftAsync(int id, Airsoft airsoft, IFormFile? formFile) => Task.CompletedTask;
        public Task DeleteAirsoftAsync(int id) => Task.CompletedTask;
        public Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft, IFormFile? formFile) => Task.FromResult(new ResponseData<Airsoft>());
    }
}
