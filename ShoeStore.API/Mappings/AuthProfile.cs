using AutoMapper;
using ShoeStore.API.Models;
using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Models.Entities;

namespace ShoeStore.API.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // Map từ RegisterDto -> User
            // Password sẽ được hash trong AfterMap
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Sẽ set trong AfterMap
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => Role.User))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.EmailVerificationToken, opt => opt.Ignore()) // Sẽ set trong AfterMap
                .ForMember(dest => dest.EmailVerificationTokenExpiry, opt => opt.Ignore()) // Sẽ set trong AfterMap
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore())
                .ForMember(dest => dest.GoogleId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
