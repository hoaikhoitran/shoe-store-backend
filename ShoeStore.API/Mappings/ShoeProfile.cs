using AutoMapper;
using ShoeStore.API.Models.Entities;
using ShoeStore.API.Models.DTOs;

namespace ShoeStore.API.Mappings
{
    public class ShoeProfile : Profile
    {
        // AutoMapper Profile:
        // Định nghĩa rule mapping giữa Entity và DTO
        // Giúp tách mapping logic ra khỏi Service / Controller
        public ShoeProfile()
        {
            // Map từ Entity -> Response DTO
            // Dùng khi trả dữ liệu từ DB ra frontend
            // Chỉ map các field có trong ShoeResponseDto
            // => Ẩn các field nhạy cảm như IsDeleted, Internal fields
            CreateMap<Shoe, ShoeResponseDto>();
            // Map từ Create DTO -> Entity
            // Dùng khi frontend gửi dữ liệu tạo mới
            CreateMap<ShoeCreateDto, Shoe>()
                // Không cho frontend set Id
                // Id do Database tự sinh (tránh fake / security hole)
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                // Backend tự set thời gian tạo
                // Frontend không cần gửi CreatedAt
                // Dùng UtcNow để chuẩn server / production
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
