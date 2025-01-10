using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace API_FashionWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly I_OrderService _orderService;
        private readonly I_OrderDetailService _orderDetailService;
        public OrdersController(I_OrderService orderService, I_OrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }
        /* 
         Danh sách các controller: 
          + Order
            - 1.Lấy về danh sách tất cả các đơn hàng (Admin)
            - 2.Lấy về một đơn hàng bằng OrderId (Admin)
            - 3.Lấy về danh sách các đơn hàng theo trạng thái (Admin)
            - 4.Cập nhật trạng thái đơn hàng (Admin)
            - 5.Xóa đơn hàng (Admin)
            - 6.Lấy về danh sách các đơn hàng theo UserId (Admin, User)
            - 7.Lấy về một đơn hàng theo UserId và OrderId (User)
            - 8.Thêm đơn hàng mới (User)
            - 9.Hủy đơn hàng (User)
          + OrderDetail
            - 10.Lấy về tất cả các OrderDetail trong 1 đơn hàng (có chung orderId)
            - 11.Lấy về orderDetail by Id
            - 12.Xóa OrderDetail (Dùng khi khách hàng đã đặt nhiều mặt hàng trong 1 đơn nhưng cửa hàng phát hiện
              không còn sản phẩm đó hoặc sản phẩm bị lỗi ... ko thể giao)
            - 13.Xóa nhiều OrderDetail có chung orderID và cập nhật trạng thái order: Order.Status = Canceled
         */

        // 1.Lấy về danh sách tất cả các đơn hàng(Admin)

        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrder()
        {
            var listOrders = await _orderService.GetAllOrders();
            if (listOrders != null && listOrders.Any())
            {
                var listOrderVm = new List<OrderViewModel>();
                foreach (var order in listOrders)
                {
                    listOrderVm.Add(new OrderViewModel
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        ShippingFee = order.ShippingFee,
                        PaymentMethod = order.PaymentMethod,
                        ShippingAddress = order.ShippingAddress,
                        CreatedAt = order.CreatedAt,
                        UpdatedAt = order.UpdatedAt,
                        OrderDetailIds = order.OrderDetails.Select(od=>od.Id).ToList(),
                    });
                }
                return Ok(listOrderVm);
            }
            return NotFound();
        }
        // 2.Lấy về một đơn hàng bằng OrderId(Admin)
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order != null)
            {
                var orderVm = new OrderViewModel()
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    ShippingFee = order.ShippingFee,
                    PaymentMethod = order.PaymentMethod,
                    ShippingAddress = order.ShippingAddress,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderDetailIds = order.OrderDetails.Select(od => od.Id).ToList(),
                };
                return Ok(orderVm);
            }
            return NotFound();
        }
        // 3.Lấy về danh sách các đơn hàng theo trạng thái(Admin)
        [HttpGet("get-orders-by-status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(OrderStatus status)
        {
            var listOrders = await _orderService.GetOrderByStatus(status);
            if (listOrders != null && listOrders.Any())
            {
                var listOrderVm = new List<OrderViewModel>();
                foreach (var order in listOrders)
                {
                    listOrderVm.Add(new OrderViewModel
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        ShippingFee = order.ShippingFee,
                        PaymentMethod = order.PaymentMethod,
                        ShippingAddress = order.ShippingAddress,
                        CreatedAt = order.CreatedAt,
                        UpdatedAt = order.UpdatedAt,
                        OrderDetailIds = order.OrderDetails.Select(od => od.Id).ToList(),
                    });
                }
                return Ok(listOrderVm);
            }
            return NotFound();
        }
        // 4.Cập nhật trạng thái đơn hàng(Admin)
        [HttpPut("update-order/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            try
            {
                _orderService.UpdateOrderStatus(orderId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Update failed: {ex.Message}" });
            }
        }
        // 5.Xóa đơn hàng(Admin)
        [HttpDelete("delete-order-by-id/{id}")]
        public async Task<IActionResult> DeleteOrderById(Guid id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Delete failed: {ex.Message}" });
            }
        }
        // 6.Lấy về danh sách các đơn hàng theo UserId(Admin, User)
        [HttpGet("get-orders-by-userid/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            var listOrders =  await _orderService.GetOrdersByUserId(userId);
            if (listOrders != null && listOrders.Any())
            {
                var listOrderVm = new List<OrderViewModel>();
                foreach (var order in listOrders)
                {
                    listOrderVm.Add(new OrderViewModel
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        ShippingFee = order.ShippingFee,
                        PaymentMethod = order.PaymentMethod,
                        ShippingAddress = order.ShippingAddress,
                        CreatedAt = order.CreatedAt,
                        UpdatedAt = order.UpdatedAt,
                        OrderDetailIds = order.OrderDetails.Select(od => od.Id).ToList(),
                    });
                }
                return Ok(listOrderVm);
            }
            return NotFound();
        }
        // 7.Lấy về một đơn hàng theo UserId và OrderId(User)
        [HttpGet("get-order-for-user-by-id/{orderId}")]
        public async Task<IActionResult> GetOrderByIdForUser(string userId, Guid orderId)
        {
            var order = await _orderService.GetOrderByIdForUser(userId, orderId);
            if (order != null)
            {
                var orderVm = new OrderViewModel()
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    ShippingFee = order.ShippingFee,
                    PaymentMethod = order.PaymentMethod,
                    ShippingAddress = order.ShippingAddress,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderDetailIds = order.OrderDetails.Select(od => od.Id).ToList(),
                };
                return Ok(orderVm);
            }
            return NotFound();
        }
        // 8.Thêm đơn hàng mới(User)
        [HttpPost("add-new-order")]
        public async Task<IActionResult> AddNewOrder([FromBody] Add_OrderViewModel orderVm)
        {
            try
            {
                await _orderService.AddOrder(orderVm);
                return Ok(orderVm);
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Add failed: {ex.Message}" });
            }
        }
        // 9.Hủy đơn hàng(User)
        [HttpPut("cancel-order/{id}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            try
            {
                await _orderService.CancelOrder(id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Cancel order failed: {ex.Message}" });
            }
        }
        // 10.Lấy về tất cả các OrderDetail trong 1 đơn hàng(có chung orderId)
        [HttpGet("get-orderdetails-by-orderid/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var listODs = await _orderDetailService.GetOrderDetailsByOderId(orderId);
            if (listODs != null && listODs.Any())
            {
                var listODsVm = new List<OrderDetailViewModel>();
                foreach (var od in listODs)
                {
                    listODsVm.Add(new OrderDetailViewModel
                    {
                        Id = od.Id,
                        OrderId = od.OrderId,
                        ProductVariantId = od.ProductVariantId,
                        Quantity = od.Quantity,
                        Price = od.Price,
                        TotalPrice = od.TotalPrice,
                    });
                }
                return Ok(listODsVm);
            }
            return NotFound();
        }
        // 11.Lấy về orderDetail by Id
        [HttpGet("get-orderdetail-by-order-detail-id/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailByOrderDetailId(Guid orderDetailId)
        {
            var od = await _orderDetailService.GetOrderDetailByOrderDetailId(orderDetailId);
            if (od != null)
            {
                var odVm = new OrderDetailViewModel()
                {
                    Id = od.Id,
                    OrderId = od.OrderId,
                    ProductVariantId = od.ProductVariantId,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    TotalPrice = od.TotalPrice,
                };
                return Ok(odVm);
            }
            return NotFound();
        }
        // 12.Xóa OrderDetail(Dùng khi khách hàng đã đặt nhiều mặt hàng trong 1 đơn nhưng cửa hàng phát hiện không còn sản phẩm đó hoặc sản phẩm bị lỗi ... ko thể giao)
        [HttpDelete("delete-order-detailby-id/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetailById(Guid orderDetailId)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetail(orderDetailId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Delete failed: {ex.Message}" });
            }
        }
    }
}
