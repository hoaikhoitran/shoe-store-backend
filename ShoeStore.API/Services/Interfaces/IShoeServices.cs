using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Models.Entities;

namespace ShoeStore.API.Services.Interfaces
{
    public interface IShoeServices
    {
        // Các method dùng cho user
        Task<PagedResult<ShoeResponseDto>> GetShoesAsync(FilterParams filters, int pageNumber, int pageSize);
        Task<ShoeResponseDto> GetShoeByIdAsync(int id);
        // Các method dùng cho admin
        Task<ShoeResponseDto> CreateShoeAsync(ShoeCreateDto dto);
        Task<ShoeResponseDto> UpdateShoeAsync(int id, ShoeCreateDto dto);
        Task<ShoeResponseDto> DeleteShoeAsync(int id);

    }
}
