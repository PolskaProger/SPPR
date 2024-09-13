using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Services
{
    public interface ICategoryService
    {
        Task<ResponseData<ListModel<Category>>> GetCategoryListAsync();
    }
}
