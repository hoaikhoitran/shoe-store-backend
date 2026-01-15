using Microsoft.EntityFrameworkCore;
using ShoeStore.API.Data;
using ShoeStore.API.Models.Entities;
using ShoeStore.API.Repositories.IShoeRepository;

namespace ShoeStore.API.Repositories.ShoeRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ShoeStoreDbContext _context;
        public UserRepository(ShoeStoreDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
           => await _context.Users.FirstOrDefaultAsync(x => x.Username == username && !x.IsDeleted);

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
            => await _context.Users.FirstOrDefaultAsync(x =>
                x.RefreshToken == refreshToken &&
                x.RefreshTokenExpiryTime > DateTime.UtcNow);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
