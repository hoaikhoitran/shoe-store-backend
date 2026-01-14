using Microsoft.EntityFrameworkCore;
using ShoeStore.API.Data;
using ShoeStore.API.Models.Entities;
using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Repositories.IShoeRepository;

namespace ShoeStore.API.Repositories.IShoeRepository
{
    public class ShoeRepository : IShoeRepository
    {
        ShoeStoreDbContext _context;

        public ShoeRepository(ShoeStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Shoe> GetShoeByIdAsync(int id)
        {
            return await _context.Shoes.FindAsync(id);
        }

        public async Task<IEnumerable<Shoe>> GetAllAsync()
        {
            return await _context.Shoes.ToListAsync();
        }

        public async Task<Shoe> AddAsync(Shoe shoe)
        {
            _context.Shoes.Add(shoe);
            await _context.SaveChangesAsync();
            return shoe;
        }

        public async Task UpdateAsync(Shoe shoe)
        {
            _context.Shoes.Update(shoe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shoe = await _context.Shoes.FindAsync(id);
            if (shoe != null)
            {
                _context.Shoes.Remove(shoe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Shoes.AnyAsync(s => s.Id == id);
        }

        public async Task<PagedResult<Shoe>> GetPageAsync(int pageNumber, int pageSize = 10)
        {
            var totalItems = await _context.Shoes.CountAsync();
            var items = await _context.Shoes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResult<Shoe>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }
    }
}
