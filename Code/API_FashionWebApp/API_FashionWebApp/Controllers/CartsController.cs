using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FashionWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly I_CartService _cartService;
        public CartsController (I_CartService cartService)
        {
            _cartService = cartService;
        }
// Get controllers
        // Lấy tất cả các sản phẩm được thêm vào giỏ hàng (ít dùng). Dùng để lọc ra các sản phẩm khách hàng quan tâm hoặc không quan tâm
        [HttpGet("get-all-carts")]
        public async Task<IActionResult> GetAllProductInCart()
        {
            var productsInCart = await _cartService.GetAllCarts();
            if (productsInCart != null)
            {
                var productsInCartVm = new List<CartViewModel>();
                foreach (var cartItem in productsInCart)
                {
                    productsInCartVm.Add(new CartViewModel
                    {
                        Id = cartItem.Id,
                        UserId = cartItem.UserId,
                        ProductVariantId = cartItem.ProductVariantId,
                        Quantity = cartItem.Quantity,
                        AddedAt = cartItem.AddedAt,
                    });
                }
                return Ok(productsInCartVm);
            }
            return NotFound();
        }
        // Lấy về 1 product in cart (1 item trong giỏ hàng) // Ko dùng đến
        //[HttpGet("get-cart-by-id/{id}")]
        //public async Task<IActionResult> GetProductInCartById(Guid id){}

        // Lấy về 1 product in cart (1 item trong giỏ hàng) cho người dùng
        [HttpGet("get-cart-by-userid-and-cartid/{userId}/{cartId}")]
        public async Task<IActionResult> GetCartByUserIdAndCartId(string userId, Guid cartId)
        {
            var cart = await _cartService.GetCartByUserIdAndCartId(userId, cartId);
            if (cart != null)
            {
                var cartVm = new CartViewModel()
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    ProductVariantId = cart.ProductVariantId,
                    Quantity = cart.Quantity,
                    AddedAt = cart.AddedAt,
                };
                return Ok(cartVm);
            }
            return NotFound();
        }

// Add controllers
        [HttpPost("add-product-to-cart/{userId}")]
        public async Task<IActionResult> AddProductToCart(string userId, [FromBody] Add_CartViewModel cartVm)
        {
            if (userId != cartVm.UserId)
            {
                return BadRequest(new { Message = "UserId in URL does not match UserId in body." });
            }
            try
            {
                await _cartService.AddCart(cartVm);
                return Ok(cartVm);
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Add failed: {ex.Message}" });
            }
        }


// Update Controller
        [HttpPut("update-quantity-cart/{cartId}")]
        public async Task<IActionResult> UpdateQuantityCart(Guid cartId, int newQuantity)
        {
            try
            {
                await _cartService.UpdateQuantityCart(cartId, newQuantity);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Update failed: {ex.Message}" });
            }
        }

// Delete Controller
        // Xóa 1 sản phẩm trong giỏ hàng
        [HttpDelete("delete-cartItem-by-id/{cartId}")]
        public async Task<IActionResult> DeleteCart(Guid cartId)
        {
            try
            {
                await _cartService.DeleteCartItem(cartId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Delete failed: {ex.Message}" });
            }
        }

        // Xóa tất cả các sản phẩm trong giỏ hàng của user
        [HttpDelete("delete-carts-by-userid/{userId}")]
        public async Task<IActionResult> DeleteCartItemsByUserId(string userId)
        {
            try
            {
                await _cartService.DeleteCartItemsByUserId(userId);
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
