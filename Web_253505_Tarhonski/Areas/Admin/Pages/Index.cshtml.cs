using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Sevices.AirsoftService.Web_253505_Tarhonski.UI.Services.AirsoftService;

namespace Web_253505_Tarhonski.Areas.Admin.Pages
{
    [Area("Admin")]
    [Route("Admin/Index")]
    public class IndexModel : PageModel
    {
        private readonly IAirsoftService _airsoftService;

        public IndexModel(IAirsoftService airsoftService)
        {
            _airsoftService = airsoftService;
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IList<Web_253505_Tarhonski.Domain.Entities.Airsoft> Airsoft { get; set; } = default!;

        public async Task OnGetAsync(int pageNo = 1)
        {
            // Получаем данные с учётом номера страницы и размера страницы
            var airsoftResponse = await _airsoftService.GetAirsoftListAsync(null, pageNo);

            if (airsoftResponse.Successfull)
            {
                Airsoft = airsoftResponse.Data.Items;
                CurrentPage = airsoftResponse.Data.CurrentPage;
                TotalPages = airsoftResponse.Data.TotalPages;
            }
        }

    }
}
