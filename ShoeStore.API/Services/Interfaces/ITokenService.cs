using ShoeStore.API.Models.Entities;

namespace ShoeStore.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
