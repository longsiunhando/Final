using API_FashionWebApp.Data;
using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API_FashionWebApp.Services
{
    public class CartService : I_CartService
    {
        private readonly db_FashionWebApp _context;
        public CartService(db_FashionWebApp context)
        {
            _context = context;
        }
        //Lấy danh sách tất cả các Cart
        public async Task<List<Cart>> GetAllCarts()
        {
            return await _context.Carts.ToListAsync();
        }
        // Lấy Cart theo cartId và userId
        public async Task<Cart> GetCartByUserIdAndCartId(string userId, Guid cartId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == cartId);
        }
        // Lấy Cart theo id
        public async Task<Cart> GetCartById(int id)
        {
            return await _context.Carts.FindAsync(id);
        }
        // Lấy danh sách Cart theo UserId (danh sách các sản phẩm trong giỏ hàng)
        public async Task<List<Cart>> GetCartByUserId(string UserId)
        {
            return await _context.Carts.Where(x => x.UserId == UserId).ToListAsync();
        }
        // Thêm 1 sản phẩm mới vào giỏ hàng
        public async Task AddCart(Add_CartViewModel CartVm)
        {
            // Kiểm tra user có user id có tồn tại không
            if (_context.Users.FindAsync(CartVm.UserId) != null)
            {
                if (CartVm.Quantity > 0)
                {
                    // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
                    if(await _context.Carts.FindAsync(CartVm.ProductVariantId) == null)
                    {
                        var Cart = new Cart
                        {
                            Id = Guid.NewGuid(),
                            UserId = CartVm.UserId,
                            ProductVariantId = CartVm.ProductVariantId,
                            Quantity = CartVm.Quantity,
                            AddedAt = DateTime.UtcNow,
                        };
                        _context.Carts.AddAsync(Cart);
                        await _context.SaveChangesAsync();
                    }
                    // Nếu đã tồn tại thì cập nhật số lượng sản phẩm
                    else
                    {
                        var Cart = await _context.Carts.FindAsync(CartVm.ProductVariantId);
                        Cart.Quantity += CartVm.Quantity;
                        _context.Carts.Update(Cart);
                        await _context.SaveChangesAsync();
                    }    
                }
                else throw new Exception("Quantity must be greater than 0");
            }
            else throw new Exception("UserId not found");



        }
        // Cập nhật Cart (Cập nhật số lượng sản phẩm trong giỏ)
        public async Task UpdateQuantityCart(Guid id, int newQuantity)
        {
            var Cart = await _context.Carts.FindAsync(id);
            if (newQuantity>0)
            {
                if (Cart != null)
                {
                    Cart.Quantity = newQuantity;
                    _context.Carts.Update(Cart);
                    await _context.SaveChangesAsync();
                }
                else
                    throw new Exception("Cart not found");
            }
            else
                throw new Exception("Quantity must be greater than 0");
        }
        // Xóa Cart (Xóa sản phẩm trong giỏ hàng)
        public async Task DeleteCartItem(Guid id)
        {
            var Cart = await _context.Carts.FindAsync(id);
            if (Cart != null)
            {
                _context.Carts.Remove(Cart);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Cart not found");
        }
        // Xóa tất cả các sản phẩm trong giỏ hàng của user
        public async Task DeleteCartItemsByUserId(string userId)
        {
            var listCart = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            if (listCart != null && listCart.Any())
            {
                _context.Carts.RemoveRange(listCart);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Cart not found");
        }

    }
}
