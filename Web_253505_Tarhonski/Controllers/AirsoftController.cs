using Microsoft.AspNetCore.Mvc;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Extentions;
using Web_253505_Tarhonski.Services.AirsoftService;
using Web_253505_Tarhonski.Services.CategoryService;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;
using Web_253505_Tarhonski.Sevices.CategoryService;

namespace Web_253505_Tarhonski.Controllers
{
    public class AirsoftController : Controller
    {
        private readonly IAirsoftService _airsoftService;
        private readonly ICategoryService _categoryService;

        public AirsoftController(IAirsoftService airsoftService, ICategoryService categoryService)
        {
            _airsoftService = airsoftService;
            _categoryService = categoryService;
        }

        // Метод для отображения списка с фильтрацией по категории и пагинацией
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var airsoftResponse = await _airsoftService.GetAirsoftListAsync(category, pageNo);

            if (!airsoftResponse.Successfull)
            {
                return NotFound(airsoftResponse.ErrorMessage);
            }

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
            {
                return NotFound(categoriesResponse.ErrorMessage);
            }

            // Передача категорий и текущей категории в ViewData
            ViewData["Categories"] = categoriesResponse.Data.Items;
            ViewData["CurrentCategory"] = categoriesResponse.Data.Items
                                          .FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";


            if (!Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/_Partial_AirsoftList", airsoftResponse.Data);
            }

            return View(airsoftResponse.Data);
        }


    }
}
