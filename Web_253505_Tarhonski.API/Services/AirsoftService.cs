using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski.API.Data;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Services
{
    public class AirsoftService : IAirsoftService
    {
        private readonly AppDbContext _context;

        public AirsoftService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 6)
        {
            var query = _context.Airsofts.AsQueryable();

            if (!string.IsNullOrEmpty(categoryNormalizedName) && categoryNormalizedName != "Все")
            {
                query = query.Where(a => a.Category.NormalizedName == categoryNormalizedName);
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

            return new ResponseData<ListModel<Airsoft>>
            {
                Data = new ListModel<Airsoft>
                {
                    Items = items,
                    TotalCount = totalItems,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                Successfull = true
            };
        }

        public async Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(int id)
        {
            var airsoft = await _context.Airsofts.FindAsync(id);
            if (airsoft == null)
            {
                return new ResponseData<Airsoft> { Successfull = false };
            }
            return new ResponseData<Airsoft> { Data = airsoft, Successfull = true };
        }

        public async Task UpdateAirsoftAsync(int id, Airsoft airsoft, IFormFile? formFile)
        {
            var existingAirsoft = await _context.Airsofts.FindAsync(id);
            if (existingAirsoft == null)
            {
                return;
            }

            existingAirsoft.Name = airsoft.Name;
            existingAirsoft.Description = airsoft.Description;
            existingAirsoft.Price = airsoft.Price;

            if (formFile != null)
            {
                var imageUrl = await SaveImageAsync(id, formFile);
                existingAirsoft.ImagePath = imageUrl.Data;
            }

            _context.Airsofts.Update(existingAirsoft);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAirsoftAsync(int id)
        {
            var airsoft = await _context.Airsofts.FindAsync(id);
            if (airsoft != null)
            {
                _context.Airsofts.Remove(airsoft);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft, IFormFile? formFile)
        {
            if (formFile != null)
            {
                var imageUrl = await SaveImageAsync(airsoft.ID, formFile);
                airsoft.ImagePath = imageUrl.Data;
            }

            _context.Airsofts.Add(airsoft);
            await _context.SaveChangesAsync();

            return new ResponseData<Airsoft> { Data = airsoft, Successfull = true };
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var filePath = Path.Combine("wwwroot", "Images", $"{id}_{formFile.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return new ResponseData<string> { Data = filePath, Successfull = true };
        }
    }
}
