using API_FashionWebApp.Data;
using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API_FashionWebApp.Services
{
    public class ProductService : I_ProductService
    {
        private readonly db_FashionWebApp _dbContext;
        public ProductService(db_FashionWebApp dbContext)
        {
            _dbContext = dbContext;
        }
        //Lấy danh sách tất cả các Product
        public async Task<List<Product>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        // Lấy product theo cateId
        public async Task<List<Product>> GetProductsByCateId(Guid cateId)
        {
            return await _dbContext.Products.Where(p => p.CategoryId == cateId).ToListAsync();
        }


        // Lấy Product theo id
        public async Task<Product> GetProductById(Guid id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        // Thêm mới Product
        /* Vì mỗi product có ít nhất 1 biền thể (quần áo chỉ có 1 size hoặc 1 màu duy nhất nên khi tạo 1 product mới sẽ tạo 1 variant kèm theo. */
        public async Task AddProduct(Add_ProductViewModel ProductVm)
        {
            // Bắt đầu transaction
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Tạo đối tượng Product mới
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = ProductVm.Name,
                        ImageLink = ProductVm.ImageLink,
                        Description = ProductVm.Description,
                        Price = ProductVm.Price,
                        CategoryId = ProductVm.CategoryId,
                        IsActive = ProductVm.IsActive,
                        CreatedAt = DateTime.UtcNow,
                    };

                    // Thêm Product vào cơ sở dữ liệu
                    _dbContext.Products.Add(product);
                    await _dbContext.SaveChangesAsync(); // Lưu Product để lấy Id

                    // Kiểm tra nếu có ProductVariants
                    if (ProductVm.VariantsVm != null && ProductVm.VariantsVm.Any())
                    {
                        var productVariants = new List<ProductVariant>();

                        foreach (var item in ProductVm.VariantsVm)
                        {
                            // Tạo các ProductVariant cho sản phẩm này
                            var productVariant = new ProductVariant
                            {
                                ProductId = product.Id, // Gán ProductId cho ProductVariant
                                Type = item.Type,
                                Size = item.Size,
                                Price = item.Price ?? product.Price, // Nếu Price không có thì dùng giá sản phẩm chính
                                Quantity = item.Quantity
                            };
                            productVariants.Add(productVariant);
                        }

                        // Thêm tất cả ProductVariants vào cơ sở dữ liệu
                        _dbContext.ProductVariants.AddRange(productVariants);
                        await _dbContext.SaveChangesAsync(); // Lưu tất cả ProductVariants
                    }

                    // Commit transaction sau khi thêm sản phẩm và các biến thể
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw new Exception("Error during AddProduct: " + ex.Message, ex);
                }
            }
        }

        // Cập nhật Product
        public async Task UpdateProduct(Guid id, Add_ProductViewModel ProductVm)
        {
            var Product = await _dbContext.Products.FindAsync(id);
            if (Product != null)
            {
                Product.Name = ProductVm.Name;
                Product.ImageLink = ProductVm.ImageLink;
                Product.Description = ProductVm.Description;
                Product.Price = ProductVm.Price;
                Product.CategoryId = ProductVm.CategoryId;
                Product.IsActive = ProductVm.IsActive;
                Product.UpdatedAt = DateTime.UtcNow;
                _dbContext.Products.Update(Product);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new Exception("Product not found");
        }

        // Xóa Product
        public async Task DeleteProduct(Guid id)
        {
            var Product = await _dbContext.Products.FindAsync(id);
            if (Product != null)
            {
                _dbContext.Products.Remove(Product);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new Exception("Product not found");
        }
    }
}
