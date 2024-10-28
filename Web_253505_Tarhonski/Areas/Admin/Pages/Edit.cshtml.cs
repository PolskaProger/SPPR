using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;

        private readonly ICategoryService _categoryService;
        public EditModel(IAirsoftService airsoftService, ICategoryService categoryService)
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

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (categoryResponse.Successfull)
            {
                Categories = categoryResponse.Data.Items;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ошибка при загрузке категорий.");
            }

            var airsoftResponse = await _airsoftService.GetAirsoftByIdAsync(id.Value);

            if (!airsoftResponse.Successfull)
            {
                return NotFound();
            }

            Airsoft = airsoftResponse.Data;
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

            var selectedCategory = Categories.FirstOrDefault(c => c.ID == SelectedCategoryId);
            if (selectedCategory == null)
            {
                ModelState.AddModelError("SelectedCategoryId", "Выбранная категория не найдена.");
                return Page();
            }

            Airsoft.CategoryId = selectedCategory.ID;

            // Если не загружен новый файл изображения, используем старый путь
            if (ImageFile == null && string.IsNullOrEmpty(Airsoft.ImagePath))
            {
                ModelState.AddModelError("ImageFile", "Файл изображения не выбран.");
                return Page();
            }

            await _airsoftService.UpdateAirsoftAsync(Airsoft.ID, Airsoft, ImageFile);

            return RedirectToPage("./Index");
        }
    }
}
