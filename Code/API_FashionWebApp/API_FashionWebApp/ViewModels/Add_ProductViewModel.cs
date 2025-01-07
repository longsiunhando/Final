namespace API_FashionWebApp.ViewModels
{
    public class Add_ProductViewModel
    {
        public string Name { get; set; } // Tên sản phẩm
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
