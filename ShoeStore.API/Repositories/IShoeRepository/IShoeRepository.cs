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
        // Tìm kiếm nâng cao
        Task<IEnumerable<Shoe>> GetNewArrivalsAsync(int count = 10);

        // Kiểm tra tồn kho
        Task<bool> IsInStockAsync(int shoeId, int size);
        Task<int> GetStockQuantityAsync(int shoeId, int size);

    }
}
