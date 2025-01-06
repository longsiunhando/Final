using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FashionWebApp.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } // Mã danh mục (Khóa chính)

        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 255 characters.")]
        public string Title { get; set; } // Tiêu đề danh mục

        [StringLength(500, ErrorMessage = "Description can be up to 500 characters.")]
        public string? Description { get; set; } // Mô tả danh mục (có thể null)

        [Required]
        public bool IsActive { get; set; } = true; // Trạng thái danh mục (mặc định là kích hoạt)

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo danh mục

        public DateTime? UpdatedAt { get; set; } // Ngày cập nhật danh mục

        // Navigation property - Liên kết với bảng Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
