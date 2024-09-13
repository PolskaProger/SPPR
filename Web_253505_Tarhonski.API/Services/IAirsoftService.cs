using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Services
{
    public interface IAirsoftService
    {
        Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(int id);
        Task UpdateAirsoftAsync(int id, Airsoft airsoft, IFormFile? formFile);
        Task DeleteAirsoftAsync(int id);
        Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft, IFormFile? formFile);
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
