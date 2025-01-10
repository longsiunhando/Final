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
        // Mảng các biến thể sản phẩm. Khi thêm 1 sản phẩm mới cần cung cấp 1 hoặc nhiều biến thể sp
        public Add_ProductVariantViewModel[] VariantsVm { get; set; }
    }
}
