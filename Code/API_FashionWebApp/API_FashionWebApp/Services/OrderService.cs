using API_FashionWebApp.Data;
using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API_FashionWebApp.Services
{
    public class OrderService : I_OrderService
    {
        private readonly db_FashionWebApp _context;
        public OrderService(db_FashionWebApp context)
        {
            _context = context;
        }
        // Danh sách chức năng 
        // 1.Lấy về tất cả các đơn hàng (Admin)
        // 2.Lấy về đơn hàng theo Id (Admin)
        // 3.Lấy về đơn hàng theo trạng thái (Admin)
        // 4.Cập nhật trạng thái đơn hàng (Admin)
        // 5.Xóa đơn hàng (Admin)
        // 6.Lấy về danh sách các đơn hàng theo UserId (Admin, User)
        // 7.Lấy về một đơn hàng theo UserId và OrderId (User)
        // 8.Thêm đơn hàng mới (User)
        // 9.Hủy đơn hàng (User)



        // 1.Lấy về tất cả các đơn hàng (Admin)
        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }
        // 2.Lấy về đơn hàng theo Id (Admin)
        public async Task<Order> GetOrderById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }
        // 3.Lấy về đơn hàng theo trạng thái (Admin)
        public async Task<List<Order>> GetOrderByStatus(OrderStatus status)
        {
            return await _context.Orders.Where(x => x.Status == status).ToListAsync();
        }
        // 4.Cập nhật trạng thái đơn hàng (Admin)
        public async Task UpdateOrderStatus(Guid id, OrderStatus Status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = Status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order not found");
            }
        }
        // 5.Xóa đơn hàng (Admin)
        public async Task DeleteOrder(Guid id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.OrderDetails) // Đảm bảo rằng bạn đã load đầy đủ OrderDetails
                        .FirstOrDefaultAsync(o => o.Id == id);

                    if (order != null)
                    {
                        // Xóa tất cả OrderDetails liên quan
                        _context.OrderDetails.RemoveRange(order.OrderDetails);

                        // Xóa Order
                        _context.Orders.Remove(order);

                        // Lưu thay đổi và commit transaction
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        throw new Exception("Order not found");
                    }
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    throw new Exception("Error during DeleteOrder: " + ex.Message, ex);
                }
            }
        }

        // 6.Lấy về danh sách các đơn hàng theo UserId (Admin, User)
        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        // 7.Lấy về một đơn hàng theo UserId và OrderId (User)
        public async Task<Order> GetOrderByIdForUser(string UserId, Guid OderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.UserId == UserId && x.Id == OderId);
        }
        // 8.Thêm đơn hàng mới (User)
        public async Task AddOrder(Add_OrderViewModel OrderVm)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = new Order
                    {
                        UserId = OrderVm.UserId,
                        OrderDate = DateTime.UtcNow,
                        Status = OrderStatus.Pending,
                        TotalPrice = 0,
                        ShippingFee = OrderVm.ShippingFee,
                        PaymentMethod = OrderVm.PaymentMethod,
                        ShippingAddress = OrderVm.ShippingAddress
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync(); // Lưu Order để lấy Id của Order

                    decimal totalPrice = 0;
                    var orderDetails = new List<OrderDetail>();

                    foreach (var item in OrderVm.OrderDetailsVm)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductVariantId = item.ProductVariantId,
                            Quantity = item.Quantity,
                            Price = item.Price,
                        };
                        orderDetails.Add(orderDetail);
                        totalPrice += item.TotalPrice;  // Tính tổng giá trị của đơn hàng
                    }

                    _context.OrderDetails.AddRange(orderDetails); // Thêm tất cả OrderDetails vào cơ sở dữ liệu
                    order.TotalPrice = totalPrice; // Cập nhật lại TotalPrice của Order

                    _context.Orders.Update(order); // Cập nhật Order với TotalPrice mới
                    await _context.SaveChangesAsync(); // Lưu tất cả thay đổi trong một lần

                    await transaction.CommitAsync(); // Commit transaction
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback nếu có lỗi
                    throw new Exception("Error during AddOrder: " + ex.Message, ex);
                }
            }
        }

        // 9.Hủy đơn hàng (User)
        public async Task CancelOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = OrderStatus.Canceled;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order not found");
            }
        }
    }
}
