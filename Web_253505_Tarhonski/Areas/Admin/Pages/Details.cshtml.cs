using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;
        private readonly ICategoryService _categoryService;

        public DetailsModel(IAirsoftService airsoftService, ICategoryService categoryService)
        {
            _airsoftService = airsoftService;
            _categoryService = categoryService;
        }

        public Web_253505_Tarhonski.Domain.Entities.Airsoft Airsoft { get; set; } = default!;
        public List<Category> Categories { get; set; } = new List<Category>();
        public Web_253505_Tarhonski.Domain.Entities.Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airsoftResponse = await _airsoftService.GetAirsoftByIdAsync(id.Value);

            if (!airsoftResponse.Successfull)
            {
                return NotFound();
            }
            Airsoft = airsoftResponse.Data;
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (categoryResponse.Successfull)
            {
                Categories = categoryResponse.Data.Items;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ошибка при загрузке категорий.");
                return Page();
            }
            Category = Categories.FirstOrDefault(c => c.ID == Airsoft.CategoryId);
            return Page();
        }
    }
}
