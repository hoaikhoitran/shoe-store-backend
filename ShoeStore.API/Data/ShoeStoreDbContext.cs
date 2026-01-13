using Microsoft.EntityFrameworkCore;

namespace ShoeStore.API.Data
{
    public class ShoeStoreDbContext : DbContext
    {
        public ShoeStoreDbContext(DbContextOptions<ShoeStoreDbContext> options)
            : base(options)
        {
        }

    }
}
