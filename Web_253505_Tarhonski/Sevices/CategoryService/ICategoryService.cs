using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.Sevices.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        Task<ResponseData<ListModel<Category>>> GetCategoryListAsync();
        Task<ResponseData<Category>> GetCategoryByIdAsync(Guid? id);
    }
}
