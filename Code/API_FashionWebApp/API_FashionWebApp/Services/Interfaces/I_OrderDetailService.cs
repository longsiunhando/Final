using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;

namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_OrderDetailService
    {
        // Lấy về tất cả order detail có cùng oderId
        Task<List<OrderDetail>> GetOrderDetailsByOderId(Guid orderId);
        // Lấy về orderDetail by Id
        Task<OrderDetail> GetOrderDetailByOrderDetailId(Guid orderDetailId);
        // Thêm mới OrderDetail
        Task AddOrderDetail(Guid orderId, Add_OrderDetailViewModel orderDetailVm);
        // Cập nhật OrderDetail
        //Task UpdateOrderDetail(Guid id, Add_OrderDetailViewModel orderDetailVm); 
        // Xóa OrderDetail
        Task DeleteOrderDetail(Guid id);
        // Xóa nhiều OrderDetail có chung orderID
        //Task DeleteOrderDetailsByOrderId(Guid orderId);
    }
}
