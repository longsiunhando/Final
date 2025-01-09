using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;
namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_ProductVariantService
    {
        //Lấy danh sách tất cả các ProductVariant
        Task<List<ProductVariant>> GetAllProductVariants();
        //Lấy danh sách các ProductVariant theo ProductId
        Task<List<ProductVariant>> GetProductVariantsByProductId(Guid productId);
        // Lấy ProductVariant theo id
        Task<ProductVariant> GetProductVariantById(Guid id);
        // Thêm mới ProductVariant
        Task AddProductVariant(Add_ProductVariantViewModel ProductVariantVm);
        // Cập nhật ProductVariant
        Task UpdateProductVariant(Guid id, Add_ProductVariantViewModel ProductVariantVm);
        // Xóa 1 ProductVariant
        Task DeleteProductVariant(Guid id);
        // Xóa nhiều ProductVariant có ProductId = productId
        Task DeleteProductVariantsByProductId(Guid productId);
    }
}
