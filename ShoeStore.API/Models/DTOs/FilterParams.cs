namespace ShoeStore.API.Models.DTOs
{
    public class FilterParams
    {
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Size { get; set; }
        public string Color { get; set; }
        public string SortBy { get; set; } // "price_asc", "price_desc"
    }
}
