using System.Collections.Generic;
using System.Threading.Tasks;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        private List<Category> _categories;

        public MemoryCategoryService()
        {
            SetupData(); // Инициализация списка категорий
        }

        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        public Task<ResponseData<ListModel<Category>>> GetCategoryListAsync()
        {
            var response = new ResponseData<ListModel<Category>>
            {
                Data = new ListModel<Category>
                {
                    Items = _categories,
                    TotalCount = _categories.Count
                },
                Successfull = true
            };

            return Task.FromResult(response);
        }

        /// <summary>
        /// Инициализация данных категорий
        /// </summary>
        private void SetupData()
        {
            _categories = new List<Category>
            {
                new Category
                {
                    ID = 1,
                    Name = "Автоматы",
                    NormalizedName = "rifles",
                },
                new Category
                {
                    ID = 2,
                    Name = "Пистолеты",
                    NormalizedName = "pistols",
                },
                new Category
                {
                    ID = 3,
                    Name = "Дробовики",
                    NormalizedName = "shotguns",
                },
                new Category
                {
                    ID = 4,
                    Name = "Пулемёты",
                    NormalizedName = "machineguns",
                }
            };
        }
    }
}
