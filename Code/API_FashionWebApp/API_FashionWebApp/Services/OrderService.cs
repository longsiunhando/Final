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
        // 6.Lấy về đơn hàng theo UserId (Admin, User)
        // 7.Thêm đơn hàng mới (User)
        // 8.Hủy đơn hàng (Admin, User)



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
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order not found");
            }
        }
        // 6.Lấy về đơn hàng theo UserId (Admin, User)
        public async Task<List<Order>> GetOrderByUserId(string UserId, Guid OderId)
        {
            return await _context.Orders.Where(x => x.UserId == UserId && x.Id == OderId).ToListAsync();
        }
        // 7.Thêm đơn hàng mới (User)
        public async Task AddOrder(Add_OrderViewModel OrderVm, Add_OrderDetailViewModel[] listOderDetailVm)
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
            await _context.SaveChangesAsync();
            decimal totalPrice = 0;
            foreach (var item in listOderDetailVm)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                };
                _context.OrderDetails.Add(orderDetail);
                // Tính tổng giá trị đơn hàng dựa vào việc lặp qua các sản phẩm có trong đơn hàng
                totalPrice += item.TotalPrice; 
            }
            order.TotalPrice = totalPrice;
            await _context.SaveChangesAsync();
            
        }
        // 8.Hủy đơn hàng (Admin, User)
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
