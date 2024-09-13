using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski.Domain.Entities;

namespace Web_253505_Tarhonski.API.Data
{
    public class AppDbContext : DbContext
    {
        // Конструктор, принимающий DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Коллекции сущностей
        public DbSet<Airsoft> Airsofts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Дополнительная настройка моделей, если необходимо
            base.OnModelCreating(modelBuilder);
        }
    }
}
