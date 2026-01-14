namespace ShoeStore.API.Models.DTOs
{
    public class ShoeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Size { get; set; }
        public string Color { get; set; } = null!;
        public string? Description { get; set; }
    }
}
