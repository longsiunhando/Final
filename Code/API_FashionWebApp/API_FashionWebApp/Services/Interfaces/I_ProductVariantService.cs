using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;
namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_ProductVariantService
    {
        //Lấy danh sách tất cả các ProductVariant
        Task<List<ProductVariant>> GetAllProductVariants();
        // Lấy ProductVariant theo id
        Task<ProductVariant> GetProductVariantById(int id);
        // Thêm mới ProductVariant
        Task AddProductVariant(Add_ProductVariantViewModel ProductVariantVm);
        // Cập nhật ProductVariant
        Task UpdateProductVariant(Guid id, Add_ProductVariantViewModel ProductVariantVm);
        // Xóa ProductVariant
        Task DeleteProductVariant(Guid id);
    }
}
