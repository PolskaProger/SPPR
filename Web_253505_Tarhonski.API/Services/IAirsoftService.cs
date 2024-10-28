using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Services
{
    public interface IAirsoftService
    {
        Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(Guid id);
        Task UpdateAirsoftAsync(Guid id, Airsoft airsoft);
        Task DeleteAirsoftAsync(Guid id);
        Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft);
        Task<ResponseData<string>> SaveImageAsync(Guid id, IFormFile formFile);
    }
}
