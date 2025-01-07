using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;
namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_CartService
    {
        //Lấy danh sách tất cả các Cart
        Task<List<Cart>> GetAllCarts();
        // Lấy Cart theo id
        Task<Cart> GetCartById(int id);
        // Lấy danh sách Cart theo UserId
        Task<List<Cart>> GetCartByUserId(string UserId);
        // Thêm mới Cart
        Task AddCart(Add_CartViewModel CartVm);
        // Cập nhật Cart
        Task UpdateCart(Guid id,  int newQuantity);
        // Xóa Cart
        Task DeleteCart(Guid id);
    }
}
