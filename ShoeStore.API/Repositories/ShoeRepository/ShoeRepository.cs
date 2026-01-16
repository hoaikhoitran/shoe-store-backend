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
            return await _context.Shoes
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<Shoe>> GetAllAsync()
        {
            return await _context.Shoes
                .Where(s => !s.IsDeleted)
                .ToListAsync();
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
            if (shoe == null)
            {
                return;
            }
            shoe.IsDeleted = true; // xóa mềm
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Shoes.AnyAsync(s => s.Id == id && !s.IsDeleted); // nhanh hơn find, chỉ kiểm tra tồn tại record tương úng hay k trả ra tru / false,
                                                                   // không load dữ liệu, find là trả về đầy đủ các cột, 
        }

        public async Task<PagedResult<Shoe>> GetPagedAsync(int pageNumber, int pageSize = 10)
        {
            var query = _context.Shoes
                .Where(s => !s.IsDeleted);

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


        public async Task<PagedResult<Shoe>> SearchAsync(FilterParams filters, int pageNumber, int pageSize)
        {
            var query = _context.Shoes.AsQueryable().Where(s => !s.IsDeleted);

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
                null or "" => query
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
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> IsInStockAsync(int shoeId, int size)
        {
            // Check đúng sản phẩm + đúng size, và còn Stock > 0
            return await _context.Shoes
                .AnyAsync(s => s.Id == shoeId && s.Size == size && s.Stock > 0 && !s.IsDeleted);
        }

        public async Task<int> GetStockQuantityAsync(int shoeId, int size)
        {
            var shoe = await _context.Shoes
                .FirstOrDefaultAsync(s => s.Id == shoeId && s.Size == size && !s.IsDeleted);

            return shoe?.Stock ?? 0;
        }
        
            

    }
}
