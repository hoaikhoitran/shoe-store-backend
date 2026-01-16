using ShoeStore.API.Models.DTOs;

namespace ShoeStore.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<bool> VerifyEmailAsync(string token);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> GoogleLoginAsync(string idToken);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
