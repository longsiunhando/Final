using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;

namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_ProductService
    {
        //Lấy danh sách tất cả các Product
        Task<List<Product>> GetAllProducts();

        // Lấy product theo cateId
        Task<List<Product>> GetProductsByCateId(Guid cateId);

        // Lấy Product theo id
        Task<Product> GetProductById(Guid id);

        // Thêm mới Product
        Task AddProduct(Add_ProductViewModel ProductVm);

        // Cập nhật Product
        Task UpdateProduct(Guid id, Add_ProductViewModel ProductVm);

        // Xóa Product
        Task DeleteProduct(Guid id);
    }
}
