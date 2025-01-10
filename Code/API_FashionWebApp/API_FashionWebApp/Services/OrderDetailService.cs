using API_FashionWebApp.Data;
using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace API_FashionWebApp.Services
{
    public class OrderDetailService : I_OrderDetailService
    {
        private readonly db_FashionWebApp _FashionWebApp;
        public OrderDetailService(db_FashionWebApp FashionWebApp)
        {
            _FashionWebApp = FashionWebApp;
        }
        // Lấy về tất cả order detail có cùng oderId
        public async Task<List<OrderDetail>> GetOrderDetailsByOderId(Guid orderId)
        {
            return await _FashionWebApp.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
        }
        // Lấy về orderDetail by Id
        public async Task<OrderDetail> GetOrderDetailByOrderDetailId(Guid orderDetailId)
        {
            return await _FashionWebApp.OrderDetails.FindAsync(orderDetailId);
        }
        // Thêm mới OrderDetail (không dùng đến. Không cho phép thêm orderDetail sau khi đã tạo Order)
        public async Task AddOrderDetail(Guid orderId, Add_OrderDetailViewModel orderDetailVm)
        {
            //var orderDetail = new OrderDetail
            //{
            //    OrderId = orderId,
            //    ProductVariantId = orderDetailVm.ProductVariantId,
            //    Quantity = orderDetailVm.Quantity,
            //    Price = orderDetailVm.Price,
            //};
            //_FashionWebApp.OrderDetails.Add(orderDetail);
            //await _FashionWebApp.SaveChangesAsync();
        }

        // Cập nhật OrderDetail (không dùng đến)
        public async Task UpdateOrderDetail(Guid id, Add_OrderDetailViewModel orderDetailVm)
        {
            //var orderDetail = await _FashionWebApp.OrderDetails.FindAsync(id);
            //if (orderDetail != null)
            //{
            //    orderDetail.Quantity = orderDetailVm.Quantity;
            //    orderDetail.Price = orderDetailVm.Price;
            //}
            //else
            //    throw new Exception("OrderDetail not found");
        }

        /* Xóa OrderDetail (Dùng khi khách hàng đã đặt nhiều mặt hàng trong 1 đơn nhưng cửa hàng phát hiện
         không còn sản phẩm đó hoặc sản phẩm bị lỗi ... ko thể giao) */
        public async Task DeleteOrderDetail(Guid id)
        {
            using (var transaction = await _FashionWebApp.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderDetail = await _FashionWebApp.OrderDetails.FindAsync(id);
                    if (orderDetail != null)
                    {
                        // Xóa OrderDetail
                        _FashionWebApp.OrderDetails.Remove(orderDetail);

                        // Lấy thông tin Order tương ứng
                        var order = await _FashionWebApp.Orders.FindAsync(orderDetail.OrderId);
                        if (order != null)
                        {
                            // Cập nhật TotalPrice của Order
                            order.TotalPrice -= orderDetail.Price;
                            _FashionWebApp.Orders.Update(order);
                        }

                        // Kiểm tra xem còn OrderDetail nào liên kết với Order không
                        var remainingOrderDetails = await _FashionWebApp.OrderDetails
                            .AnyAsync(od => od.OrderId == orderDetail.OrderId);

                        // Nếu không còn OrderDetail nào, cập nhật trạng thái đã hủy
                        if (!remainingOrderDetails)
                        {
                            order.Status = OrderStatus.Canceled;
                            _FashionWebApp.Orders.Update(order);
                        }

                        // Lưu thay đổi vào cơ sở dữ liệu
                        await _FashionWebApp.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        throw new KeyNotFoundException("OrderDetail not found");
                    }
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw new Exception("Error during DeleteOrderDetail: " + ex.Message, ex);
                }
            }
        }

        // Xóa nhiều OrderDetail có chung orderID và cập nhật trạng thái order = canceled
        //public async Task DeleteOrderDetailsByOrderId(Guid orderId)
        //{
        //    using (var transaction = await _FashionWebApp.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            // Lấy danh sách tất cả OrderDetails liên kết với OrderId
        //            var listOrderDetails = await _FashionWebApp.OrderDetails
        //                .Where(od => od.OrderId == orderId)
        //                .ToListAsync();

        //            if (listOrderDetails != null && listOrderDetails.Any())
        //            {
        //                // Xóa tất cả OrderDetails liên kết với Order
        //                _FashionWebApp.OrderDetails.RemoveRange(listOrderDetails);

        //                // Kiểm tra xem còn OrderDetail nào liên kết với Order không
        //                var remainingOrderDetails = await _FashionWebApp.OrderDetails
        //                    .AnyAsync(od => od.OrderId == orderId);

        //                // Nếu không còn OrderDetail nào, cập nhật trạng thái đã hủy
        //                if (!remainingOrderDetails)
        //                {
        //                    var order = await _FashionWebApp.Orders.FindAsync(orderId);
        //                    if (order != null)
        //                    {
        //                        order.Status = OrderStatus.Canceled;
        //                        _FashionWebApp.Orders.Update(order);
        //                    }
        //                }

        //                // Lưu thay đổi vào cơ sở dữ liệu
        //                await _FashionWebApp.SaveChangesAsync();
        //                await transaction.CommitAsync();
        //            }
        //            else
        //            {
        //                throw new Exception("OrderDetail not found");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Rollback transaction nếu có lỗi
        //            await transaction.RollbackAsync();
        //            throw new Exception("Error during DeleteOrderDetailsByOrderId: " + ex.Message, ex);
        //        }
        //    }
        //}

    }
}
