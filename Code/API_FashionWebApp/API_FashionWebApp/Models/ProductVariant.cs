using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FashionWebApp.Models
{
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; } // Mã biến thể sản phẩm (Khóa chính)

        [ForeignKey("Product")]
        public Guid ProductId { get; set; } // Mã sản phẩm (Khóa ngoại)

        [Required]
        public Product Product { get; set; } // Navigation property tới Product

        [StringLength(100, ErrorMessage = "Type must be a maximum of 100 characters.")]
        public string Type { get; set; } // Loại sản phẩm (màu sắc, chất liệu, hình dạng)

        [StringLength(20, ErrorMessage = "Size must be a maximum of 20 characters.")]
        public string Size { get; set; } // Kích thước sản phẩm

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; } // Giá của biến thể sản phẩm, có thể có giá trị null

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0.")]
        public int Quantity { get; set; } = 0; // Số lượng tồn kho của biến thể sản phẩm
    }
}
