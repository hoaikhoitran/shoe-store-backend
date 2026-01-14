using ShoeStore.API.Models.Entities;
using ShoeStore.API.Models.DTOs;

namespace ShoeStore.API.Repositories.IShoeRepository
{
    public interface IShoeRepository
    {
        // CRUD cơ bản
        Task<Shoe> GetByIdAsync(int id);
        Task<IEnumerable<Shoe>> GetAllAsync();
        Task<Shoe> AddAsync(Shoe shoe);
        Task UpdateAsync(Shoe shoe);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        // Phân trang & Tìm kiếm
        Task<PagedResult<Shoe>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagedResult<Shoe>> SearchAsync(FilterParams filters, int pageNumber, int pageSize);

        // Lọc theo danh mục
        Task<PagedResult<Shoe>> GetByBrandAsync(string brand, int pageNumber, int pageSize);
        Task<PagedResult<Shoe>> GetByCategoryAsync(string category, int pageNumber, int pageSize);
        Task<PagedResult<Shoe>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, int pageNumber, int pageSize);

        // Tìm kiếm nâng cao
        Task<IEnumerable<Shoe>> GetFeaturedShoesAsync(int count = 10);
        Task<IEnumerable<Shoe>> GetNewArrivalsAsync(int count = 10);
        Task<IEnumerable<Shoe>> GetBestSellersAsync(int count = 10);
        Task<IEnumerable<Shoe>> GetRelatedShoesAsync(int shoeId, int count = 4);

        // Kiểm tra tồn kho
        Task<bool> IsInStockAsync(int shoeId, int size);
        Task<int> GetStockQuantityAsync(int shoeId, int size);

        // Thống kê
        Task<IEnumerable<int>> GetAvailableSizesAsync(int shoeId);
    }
}
