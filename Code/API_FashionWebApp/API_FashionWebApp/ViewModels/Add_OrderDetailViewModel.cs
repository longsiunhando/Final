namespace API_FashionWebApp.ViewModels
{
    public class Add_OrderDetailViewModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
