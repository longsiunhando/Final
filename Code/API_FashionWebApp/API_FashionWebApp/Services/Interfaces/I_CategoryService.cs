using API_FashionWebApp.Models;
using API_FashionWebApp.ViewModels;
namespace API_FashionWebApp.Services.Interfaces
{
    public interface I_CategoryService
    {
        //Lấy danh sách tất cả các category
        Task<List<Category>> GetAllCategories();

        // Lấy category theo id
        Task<Category> GetCategoryById(Guid id);

        // Thêm mới category
        Task AddCategory(Add_CategoryViewModel categoryVm);

        // Cập nhật category
        Task UpdateCategory(Guid id, Add_CategoryViewModel categoryVm);

        // Xóa category
        Task DeleteCategory(Guid id);
    }
}
