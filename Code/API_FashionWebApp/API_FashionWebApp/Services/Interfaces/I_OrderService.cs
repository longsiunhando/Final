using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;

namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_OrderService
    {
        // Lấy về tất cả các đơn hàng (Admin)
        Task<List<Order>> GetAllOrders();
        // Lấy về đơn hàng theo Id (Admin)
        Task<Order> GetOrderById(Guid id);
        // Lấy về đơn hàng theo trạng thái (Admin)
        Task<List<Order>> GetOrderByStatus(OrderStatus status);
        // Cập nhật trạng thái đơn hàng (Admin)
        Task UpdateOrderStatus(Guid id, OrderStatus Status);
        // Xóa đơn hàng (Admin)
        Task DeleteOrder(Guid id);
        // Lấy về đơn hàng theo UserId (Admin, User)
        Task<List<Order>> GetOrderByUserId(string UserId, Guid OrderId);
        // Thêm đơn hàng mới (User)
        Task AddOrder(Add_OrderViewModel OrderVm, Add_OrderDetailViewModel[] listOderDetailVm);
        // Hủy đơn hàng (Admin, User)
        Task CancelOrder(Guid id);

    }
}
