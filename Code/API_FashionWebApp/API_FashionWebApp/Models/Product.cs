using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FashionWebApp.Models
{
    public class Product
    {
        [Key]
        [Required]
        public Guid Id { get; set; } // Mã sản phẩm (Khóa chính)

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Product name must be between 5 and 200 characters.")]
        public string Name { get; set; } // Tên sản phẩm

        [Required]
        public string ImageLink { get; set; } // Link ảnh chính của sản phẩm

        [Required]
        [StringLength(5000, ErrorMessage = "Description can be up to 5000 characters.")]
        public string Description { get; set; } // Mô tả sản phẩm

        [Required]
        [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá sản phẩm

        [Required]
        public Guid CategoryId { get; set; } // Mã danh mục (Khóa ngoại)

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } // Navigation property tới danh mục

        [Required]
        public bool IsActive { get; set; } = true; // Trạng thái sản phẩm (Active/Inactive)

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>(); // Các biến thể của sản phẩm

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo sản phẩm

        public DateTime? UpdatedAt { get; set; } // Ngày cập nhật sản phẩm
    }
}
