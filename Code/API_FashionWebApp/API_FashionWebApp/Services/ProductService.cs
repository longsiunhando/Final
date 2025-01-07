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

        // Lấy Product theo id
        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        // Thêm mới Product
        public async Task AddProduct(Add_ProductViewModel ProductVm)
        {
            var Product = new Product
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
            _dbContext.Products.Add(Product);
            await _dbContext.SaveChangesAsync();
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
