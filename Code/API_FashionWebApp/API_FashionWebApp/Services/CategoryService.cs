using API_FashionWebApp.Data;
using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace API_FashionWebApp.Services
{
    public class CategoryService : I_CategoryService
    {
        private readonly db_FashionWebApp _dbContext;
        public CategoryService(db_FashionWebApp dbContext)
        {
            _dbContext = dbContext;
        }

        //Lấy danh sách tất cả các category
        public async Task<List<Category>> GetAllCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        // Lấy category theo id
        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        // Thêm mới category
        public async Task AddCategory(Add_CategoryViewModel categoryVm)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Title = categoryVm.Title,
                Description = categoryVm.Description,
                IsActive = categoryVm.IsActive,
                CreatedAt = DateTime.UtcNow,
            };
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        // Cập nhật category
        public async Task UpdateCategory(Guid id, Add_CategoryViewModel categoryVm)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                category.Title = categoryVm.Title;
                category.Description = categoryVm.Description;
                category.IsActive = categoryVm.IsActive;
                category.UpdatedAt = DateTime.UtcNow;
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync();
            }
            else 
                throw new Exception("Category not found");
            
        }

        // Xóa category
        public async Task DeleteCategory(Guid id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new Exception("Category not found");
        }
    }
}
