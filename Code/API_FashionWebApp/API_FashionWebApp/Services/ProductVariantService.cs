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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tìm ProductVariant
                var productVariant = await _context.ProductVariants.FindAsync(id);
                if (productVariant != null)
                {
                    // Lấy thông tin ProductId liên kết với ProductVariant
                    var productId = productVariant.ProductId;

                    // Xóa ProductVariant
                    _context.ProductVariants.Remove(productVariant);
                    await _context.SaveChangesAsync();

                    // Kiểm tra xem sản phẩm có còn ProductVariant nào không
                    var remainingVariants = await _context.ProductVariants
                        .AnyAsync(pv => pv.ProductId == productId);

                    // Nếu không còn ProductVariant nào, xóa luôn Product
                    if (!remainingVariants)
                    {
                        var product = await _context.Products.FindAsync(productId);
                        if (product != null)
                        {
                            _context.Products.Remove(product);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Commit transaction
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("ProductVariant not found");
                }
            }
            catch
            {
                // Rollback transaction nếu gặp lỗi
                await transaction.RollbackAsync();
                throw;
            }
        }
        // Xóa nhiều ProductVariant có ProductId = productId
        public async Task DeleteProductVariantsByProductId(Guid productId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy danh sách các ProductVariant liên kết với ProductId
                var listProductVariants = await _context.ProductVariants
                    .Where(pV => pV.ProductId == productId)
                    .ToListAsync();

                if (listProductVariants != null && listProductVariants.Any())
                {
                    // Xóa tất cả các ProductVariant liên kết
                    _context.ProductVariants.RemoveRange(listProductVariants);
                    await _context.SaveChangesAsync();

                    // Kiểm tra xem sản phẩm có còn ProductVariant nào không
                    var remainingVariants = await _context.ProductVariants
                        .AnyAsync(pv => pv.ProductId == productId);

                    // Nếu không còn ProductVariant nào, xóa luôn Product
                    if (!remainingVariants)
                    {
                        var product = await _context.Products.FindAsync(productId);
                        if (product != null)
                        {
                            _context.Products.Remove(product);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Commit transaction
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception($"ProductVariant with productId = {productId} not found");
                }
            }
            catch
            {
                // Rollback transaction nếu gặp lỗi
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
