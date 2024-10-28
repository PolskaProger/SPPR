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

        public Task<ResponseData<Category>> GetCategoryByIdAsync(Guid? id) => Task.FromResult(new ResponseData<Category>());
        /// Инициализация данных категорий
        /// </summary>
        private void SetupData()
        {
            _categories = new List<Category>
            {
                new Category
                {
                    ID = Guid.NewGuid(),
                    Name = "Автоматы",
                    NormalizedName = "rifles",
                },
                new Category
                {
                    ID = Guid.NewGuid(),
                    Name = "Пистолеты",
                    NormalizedName = "pistols",
                },
                new Category
                {
                    ID = Guid.NewGuid(),
                    Name = "Дробовики",
                    NormalizedName = "shotguns",
                },
                new Category
                {
                    ID = Guid.NewGuid(),
                    Name = "Пулемёты",
                    NormalizedName = "machineguns",
                }
            };
        }
    }
}
