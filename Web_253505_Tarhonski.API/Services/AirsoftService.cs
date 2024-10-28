using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski.API.Data;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Services
{
    public class AirsoftService : IAirsoftService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public AirsoftService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 6)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Airsofts.AsQueryable();
            var dataList = new ListModel<Airsoft>();

            query = query.Where(a => categoryNormalizedName == null || a.Category!.NormalizedName.Equals(categoryNormalizedName));

            // Общее количество элементов
            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Airsoft>>.Success(dataList);
            }

            // Общее количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return ResponseData<ListModel<Airsoft>>.Error("No such page");
            }

            // Получение элементов для текущей страницы
            dataList.Items = await query.OrderBy(a => a.ID)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            // Установка текущей страницы и общего количества элементов
            dataList.CurrentPage = pageNo;
            dataList.TotalCount = count;  // Общее количество элементов, а не страниц
            dataList.TotalPages = totalPages;  // Если в модели есть TotalPages

            return ResponseData<ListModel<Airsoft>>.Success(dataList);
        }

        public async Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(Guid id)
        {
            var airsoft = await _context.Airsofts.FirstOrDefaultAsync(a => a.ID == id);

            if (airsoft is null)
            {
                return ResponseData<Airsoft>.Error($"No airsoft with id={id}");
            }

            return ResponseData<Airsoft>.Success(airsoft);
        }

        public async Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft)
        {
            await _context.Airsofts.AddAsync(airsoft);

            await _context.SaveChangesAsync();

            return ResponseData<Airsoft>.Success(airsoft);
        }

        public async Task DeleteAirsoftAsync(Guid id)
        {
            var airsoft = await _context.Airsofts.FirstOrDefaultAsync(a => a.ID == id);
            if (airsoft is null)
            {
                return;
            }

            _context.Remove(airsoft);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAirsoftAsync(Guid id, Airsoft airsoft)
        {
            var dbAirsoft = await _context.Airsofts.FirstOrDefaultAsync(a => a.ID == id);

            if (dbAirsoft is null)
            {
                return;
            }

            dbAirsoft.Price = airsoft.Price;
            dbAirsoft.Description = airsoft.Description;
            dbAirsoft.Category = airsoft.Category;
            dbAirsoft.Name = airsoft.Name;
            dbAirsoft.Category = airsoft.Category;
            dbAirsoft.ImagePath = airsoft.ImagePath;

            _context.Entry(dbAirsoft).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public Task<ResponseData<string>> SaveImageAsync(Guid id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }
}