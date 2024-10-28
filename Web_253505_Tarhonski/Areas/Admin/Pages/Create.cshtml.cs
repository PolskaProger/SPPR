using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.CategoryService;
using Web_253505_Tarhonski.API.Services;
using IAirsoftService = Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService.IAirsoftService;
using ICategoryService = Web_253505_Tarhonski.Sevices.CategoryService.ICategoryService;
using Web_253505_Tarhonski.Sevices.FileService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IAirsoftService airsoftService, ICategoryService categoryService)
        {
            _airsoftService = airsoftService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Web_253505_Tarhonski.Domain.Entities.Airsoft Airsoft { get; set; } = default!;
        [BindProperty]
        public IFormFile? ImageFile { get; set; }
        [BindProperty]
        public Guid SelectedCategoryId { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGetAsync()
        {
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (categoryResponse.Successfull)
            {
                Categories = categoryResponse.Data.Items;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ошибка при загрузке категорий.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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

            if (ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Файл изображения не выбран.");
                return Page();
            }
            var selectedCategory = Categories.FirstOrDefault(c => c.ID == SelectedCategoryId);
            if (selectedCategory == null)
            {
                ModelState.AddModelError("SelectedCategoryId", "Выбранная категория не найдена.");
                return Page();
            }

            Airsoft.CategoryId = selectedCategory.ID;

            var createResponse = await _airsoftService.CreateAirsoftAsync(Airsoft, ImageFile);
            if (!createResponse.Successfull)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при создании объекта.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}