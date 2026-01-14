using Microsoft.EntityFrameworkCore;
using ShoeStore.API.Models.Entities;

namespace ShoeStore.API.Data
{
    public class ShoeStoreDbContext : DbContext
    {
        public ShoeStoreDbContext(DbContextOptions<ShoeStoreDbContext> options)
            : base(options)
        {
        }
        public DbSet<Shoe> Shoes { get; set; }

    }
}
