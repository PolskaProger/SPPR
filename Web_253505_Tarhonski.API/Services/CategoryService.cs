using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski.API.Data;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return new ResponseData<ListModel<Category>>
            {
                Data = new ListModel<Category>
                {
                    Items = categories,
                    TotalCount = categories.Count
                },
                Successfull = true
            };
        }
    }
}
