using ShoeStore.API.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStore.API.Models.Entities
{
    public class Shoe : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        // domain-specific
        public int Size { get; set; }

        [MaxLength(50)]
        public string Color { get; set; } = null!;

        // description
        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
