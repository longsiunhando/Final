namespace API_FashionWebApp.ViewModels
{
    public class ProductVariantViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
