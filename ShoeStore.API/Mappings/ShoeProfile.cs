using AutoMapper;
using ShoeStore.API.Models.Entities;
using ShoeStore.API.Models.DTOs;

namespace ShoeStore.API.Mappings
{
    public class ShoeProfile : Profile
    {
        public ShoeProfile()
        {
            CreateMap<Shoe, ShoeResponseDto>();

            CreateMap<ShoeCreateDto, Shoe>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
