using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;

        public DetailsModel(IAirsoftService airsoftService)
        {
            _airsoftService = airsoftService;
        }

        public Web_253505_Tarhonski.Domain.Entities.Airsoft Airsoft { get; set; } = default!;

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
    }
}
