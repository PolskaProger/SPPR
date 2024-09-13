using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;

        public EditModel(IAirsoftService airsoftService)
        {
            _airsoftService = airsoftService;
        }

        [BindProperty]
        public Web_253505_Tarhonski.Domain.Entities.Airsoft Airsoft { get; set; } = default!;
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _airsoftService.UpdateAirsoftAsync(Airsoft.ID, Airsoft, ImageFile);

            return RedirectToPage("./Index");
        }
    }
}
