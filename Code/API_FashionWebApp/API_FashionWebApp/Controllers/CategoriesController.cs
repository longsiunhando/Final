using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FashionWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly I_CategoryService _categoryService;
        public CategoriesController(I_CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var categoriesVm = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                categoriesVm.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Title = category.Title,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                });
            }
            return Ok(categoriesVm);
        }

        [HttpGet("get-cate-by-id/{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category != null)
            {
                var cateVm = new CategoryViewModel()
                {
                    Id = category.Id,
                    Title = category.Title,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                };
                return Ok(cateVm);
            }
            return NotFound();
        }

        [HttpPost("add-new-cate")]
        public async Task<IActionResult> AddCate([FromBody] Add_CategoryViewModel categoryVm)
        {
            try
            {
                await _categoryService.AddCategory(categoryVm);
                return Ok(categoryVm);
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Add failed: {ex.Message}" });
            }
        }

        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] Add_CategoryViewModel cateVm)
        {
            try
            {
                await _categoryService.UpdateCategory(id, cateVm);
                return Ok(cateVm);
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Update failed: {ex.Message}" });
            }
        }

        [HttpDelete("delete-cate-by-id/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
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
