using ShoeStore.API.Models.Entities;

namespace ShoeStore.API.Repositories.IShoeRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByVerificationTokenAsync(string token);
        Task<User?> GetUserByGoogleIdAsync(string googleId);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
