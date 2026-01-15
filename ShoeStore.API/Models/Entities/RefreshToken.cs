using ShoeStore.API.Core;
using ShoeStore.API.Models.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
