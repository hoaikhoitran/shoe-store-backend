namespace ShoeStore.API.Models.DTOs
{
    public class ShoeCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int Size { get; set; }
        public string Color { get; set; } = null!;
        public string? Description { get; set; }
    }
}
