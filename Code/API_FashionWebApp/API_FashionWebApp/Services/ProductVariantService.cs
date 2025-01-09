using API_FashionWebApp.Data;
using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;
using API_FashionWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_FashionWebApp.Services
{
    public class ProductVariantService : I_ProductVariantService
    {
        private readonly db_FashionWebApp _context;
        public ProductVariantService(db_FashionWebApp context)
        {
            _context = context;
        }
        //Lấy danh sách tất cả các ProductVariant
        public async Task<List<ProductVariant>> GetAllProductVariants()
        {
            return await _context.ProductVariants.ToListAsync();
        }
        //Lấy danh sách các ProductVariant theo ProductId
        public async Task<List<ProductVariant>> GetProductVariantsByProductId(Guid productId)
        {
            return await _context.ProductVariants.Where(pv => pv.ProductId == productId).ToListAsync();
        }
        // Lấy ProductVariant theo id
        public async Task<ProductVariant> GetProductVariantById(Guid id)
        {
            return await _context.ProductVariants.FindAsync(id);
        }
        // Thêm mới ProductVariant
        public async Task AddProductVariant(Add_ProductVariantViewModel ProductVariantVm)
        {
            var ProductVariant = new ProductVariant
            {
                Id = Guid.NewGuid(),
                ProductId = ProductVariantVm.ProductId,
                Type = ProductVariantVm.Type,
                Size = ProductVariantVm.Size,
                Price = ProductVariantVm.Price,
                Quantity = ProductVariantVm.Quantity,
            };
            _context.ProductVariants.Add(ProductVariant);
            await _context.SaveChangesAsync();
        }
        // Cập nhật ProductVariant
        public async Task UpdateProductVariant(Guid id, Add_ProductVariantViewModel ProductVariantVm)
        {
            var ProductVariant = await _context.ProductVariants.FindAsync(id);
            if (ProductVariant != null)
            {
                ProductVariant.ProductId = ProductVariantVm.ProductId;
                ProductVariant.Type = ProductVariantVm.Type;
                ProductVariant.Size = ProductVariantVm.Size;
                ProductVariant.Price = ProductVariantVm.Price;
                ProductVariant.Quantity = ProductVariantVm.Quantity;
                _context.ProductVariants.Update(ProductVariant);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("ProductVariant not found");
        }
        // Xóa 1 ProductVariant
        public async Task DeleteProductVariant(Guid id)
        {
            var ProductVariant = await _context.ProductVariants.FindAsync(id);
            if (ProductVariant != null)
            {
                _context.ProductVariants.Remove(ProductVariant);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("ProductVariant not found");
        }
        // Xóa nhiều ProductVariant có ProductId = productId
        public async Task DeleteProductVariantsByProductId(Guid productId)
        {
            var listProductVariants = await _context.ProductVariants.Where(pV =>  pV.ProductId == productId).ToListAsync();
            if (listProductVariants != null)
            {
                foreach (var productVariant in listProductVariants)
                {
                    _context.ProductVariants.Remove(productVariant);
                    await _context.SaveChangesAsync();
                }
            }
            else throw new Exception($"ProductVariant with productId = {productId} not found");
        }
    }
}
