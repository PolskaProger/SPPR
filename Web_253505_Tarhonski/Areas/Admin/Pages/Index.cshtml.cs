using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;

        public IndexModel(IAirsoftService airsoftService)
        {
            _airsoftService = airsoftService;
        }

        public IList<Web_253505_Tarhonski.Domain.Entities.Airsoft> Airsoft { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var airsoftResponse = await _airsoftService.GetAirsoftListAsync(null, 1);

            if (airsoftResponse.Successfull)
            {
                Airsoft = airsoftResponse.Data.Items;
            }
        }
    }
}
