using ShoeStore.API.Core;
using ShoeStore.API.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoeStore.API.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Role Role { get; set; } = Role.User;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
