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

        public async Task<Shoe> GetByIdAsync(int id)
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

        public async Task<PagedResult<Shoe>> GetPagedAsync(int pageNumber, int pageSize = 10)
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

        public async Task<PagedResult<Shoe>> SearchAsync(FilterParams filters, int pageNumber, int pageSize)
        {
            var query = _context.Shoes.AsQueryable();

            // SearchTerm: Name / Description
            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                query = query.Where(s =>
                    s.Name.Contains(filters.SearchTerm) ||
                    (s.Description != null && s.Description.Contains(filters.SearchTerm)));
            }

            // MinPrice, MaxPrice
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(s => s.Price >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(s => s.Price <= filters.MaxPrice.Value);
            }

            // Size
            if (filters.Size.HasValue)
            {
                query = query.Where(s => s.Size == filters.Size.Value);
            }

            // Color
            if (!string.IsNullOrWhiteSpace(filters.Color))
            {
                query = query.Where(s => s.Color == filters.Color);
            }

            // SortBy
            query = filters.SortBy switch
            {
                "price_asc" => query.OrderBy(s => s.Price),
                "price_desc" => query.OrderByDescending(s => s.Price),
                "name" => query.OrderBy(s => s.Name),
                // hiện tại Shoe chưa có CreatedAt, nên "newest" fallback về Id
                "newest" => query.OrderByDescending(s => s.Id),
                _ => query.OrderBy(s => s.Id)
            };

            var totalItems = await query.CountAsync();

            var items = await query
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

        // Sản phẩm mới về
        public async Task<IEnumerable<Shoe>> GetNewArrivalsAsync(int count = 10)
        {
            return await _context.Shoes
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> IsInStockAsync(int shoeId, int size)
        {
            // Check đúng sản phẩm + đúng size, và còn Stock > 0
            return await _context.Shoes
                .AnyAsync(s => s.Id == shoeId && s.Size == size && s.Stock > 0);
        }

        public async Task<int> GetStockQuantityAsync(int shoeId, int size)
        {
            var shoe = await _context.Shoes
                .FirstOrDefaultAsync(s => s.Id == shoeId && s.Size == size);

            return shoe?.Stock ?? 0;
        }
        
            

    }
}
