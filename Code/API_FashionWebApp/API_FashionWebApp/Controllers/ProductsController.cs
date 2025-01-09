using API_FashionWebApp.Models;
using API_FashionWebApp.Services.Interfaces;
using API_FashionWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FashionWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly I_ProductService _productService;
        private readonly I_ProductVariantService _productVariantService;
        public ProductsController(I_ProductService productService, I_ProductVariantService productVariantService)
        {
            _productService = productService;
            _productVariantService = productVariantService;
        }

// Get dữ liệu
        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            if (products != null)
            {
                var productsVm = new List<ProductViewModel>();
                foreach (var product in products)
                {
                    var productVariantIds = product.Variants.Select(v => v.Id).ToList(); // Lấy danh sách Id
                    productsVm.Add(new ProductViewModel
                    {
                        Id = Guid.NewGuid(),
                        Name = product.Name,
                        ImageLink = product.ImageLink,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        IsActive = product.IsActive,
                        ProductVariantIds = productVariantIds.ToList(),
                        CreatedAt = product.CreatedAt,
                        UpdatedAt = product.UpdatedAt,
                    });
                }
                return Ok(productsVm);
            }
            return NotFound();
        }
        [HttpGet("get-product-by-cateid/{id}")]
        public async Task<IActionResult> GetProductsByCateId(Guid cateId)
        {
            var listProducts = await _productService.GetProductsByCateId(cateId);
            if (listProducts != null)
            {
                var listProductsVm = new List<ProductViewModel>();
                foreach (var product in listProducts)
                {
                    var productVariantIds = product.Variants.Select(v => v.Id).ToList(); // Lấy danh sách Id
                    listProductsVm.Add(new ProductViewModel
                    {
                        Id = Guid.NewGuid(),
                        Name = product.Name,
                        ImageLink = product.ImageLink,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        IsActive = product.IsActive,
                        ProductVariantIds = productVariantIds.ToList(),
                        CreatedAt = product.CreatedAt,
                        UpdatedAt = product.UpdatedAt,
                    });
                }
                return Ok(listProductsVm);
            }
            return NotFound();
        }
        [HttpGet("get-product-by-id/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product != null)
            {
                var productVm = new ProductViewModel()
                {
                    Id = id,
                    Name = product.Name,
                    ImageLink = product.ImageLink,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    ProductVariantIds = product.Variants.Select(v => v.Id).ToList(),
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                };
                return Ok(productVm);
            }
            return NotFound();
        }
        [HttpGet("get-all-variants")]
        public async Task<IActionResult> GetAllVariants()
        {
            var listVariants = await _productVariantService.GetAllProductVariants();
            if (listVariants != null)
            {
                var listVariantsVm = new List<ProductVariantViewModel>();
                foreach (var v in listVariants)
                {
                    listVariantsVm.Add(new ProductVariantViewModel()
                    {
                        Id = v.Id,
                        ProductId = v.ProductId,
                        Type = v.Type,
                        Size = v.Size,
                        Price = v.Price,
                        Quantity = v.Quantity,
                    });
                }
                return Ok(listVariantsVm);
            }
            return NotFound();
        }
        [HttpGet("get-variants-by-product-id/{id}")]
        public async Task<IActionResult> GetVariantsByProductId(Guid productId)
        {
            var listVariants = await _productVariantService.GetProductVariantsByProductId(productId);
            if (listVariants != null)
            {
                var listVariantsVm = new List<ProductVariantViewModel>();
                foreach (var v in listVariants)
                {
                    listVariantsVm.Add(new ProductVariantViewModel()
                    {
                        Id = v.Id,
                        ProductId = v.ProductId,
                        Type = v.Type,
                        Size = v.Size,
                        Price = v.Price,
                        Quantity = v.Quantity,
                    });
                }
                return Ok(listVariantsVm);
            }
            return NotFound();
        }
        [HttpGet("get-variants-by-id/{id}")]
        public async Task<IActionResult> GetProductVariantById(Guid id)
        {
            var variant = await _productVariantService.GetProductVariantById(id);
            if (variant != null)
            {
                var variantVm = new ProductVariantViewModel()
                {
                    Id = id,
                    ProductId = variant.ProductId,
                    Type = variant.Type,
                    Size = variant.Size,
                    Price = variant.Price,
                    Quantity = variant.Quantity,
                };
                return Ok(variantVm);
            }
            return NotFound();
        }




// Add dữ liệu
        /* Vì mỗi product có ít nhất 1 biền thể (quần áo chỉ có 1 size hoặc 1 màu duy nhất nên khi tạo 1 product mới sẽ tạo 1 variant kèm theo. */
        [HttpPost("add-new-product")]
        public async Task<IActionResult> AddNewProduct(Add_ProductViewModel productVm, Add_ProductVariantViewModel variantVm)
        {
            try
            {
                await _productService.AddProduct(productVm);
                await _productVariantService.AddProductVariant(variantVm);
                // Gộp hai tham số vào một đối tượng trả về
                return Ok(new
                {
                    Product = productVm,
                    Variant = variantVm
                });
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Add failed: {ex.Message}" });
            }
        }
        // Thêm variant vào sản phẩm
        [HttpPost("add-new-variant/{productId}")]
        public async Task<IActionResult> AddNewVariant(Guid productId, [FromBody] Add_ProductVariantViewModel variantVm)
        {
            // Kiểm tra tính nhất quán
            if (productId != variantVm.ProductId)
            {
                return BadRequest(new { Message = "ProductId in URL does not match ProductId in body." });
            }

            // Tiếp tục xử lý thêm mới variant
            try
            {
                await _productVariantService.AddProductVariant(variantVm);
                return Ok(variantVm);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Add failed: {ex.Message}" });
            }
        }



// Update dữ liệu
        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Add_ProductViewModel ProductVm)
        {
            try
            {
                await _productService.UpdateProduct(id, ProductVm);
                return Ok(ProductVm);
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Update failed: {ex.Message}" });
            }
        }
        [HttpPut("update-variant/{id}")]
        public async Task<IActionResult> UpdateVariant(Guid id, [FromBody] Add_ProductVariantViewModel VariantVm)
        {
            try
            {
                await _productVariantService.UpdateProductVariant(id, VariantVm);
                return Ok(VariantVm);
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Update failed: {ex.Message}" });
            }
        }


// Delete dữ liệu
        /* Xóa product sẽ xóa tất cả các variant có liên kết với nó. Đã thực hiện liên kết cascade trong csdl
           nhưng xây dụng code xóa variant trước khi xóa product cho chắc kèo. */
        [HttpDelete("delete-product-by-id/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {

            try
            {
                await _productVariantService.DeleteProductVariantsByProductId(productId);
                await _productService.DeleteProduct(productId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle error and return BadRequest with message
                return BadRequest(new { Message = $"Delete failed: {ex.Message}" });
            }
        }
        // Xóa 1 variant
        [HttpDelete("delete-variant-by-id/{id}")]
        public async Task<IActionResult> DeleteVariant(Guid vatiantId)
        {
            try
            {
                await _productVariantService.DeleteProductVariant(vatiantId);
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
