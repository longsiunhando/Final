using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FashionWebApp.Models
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; } // Mã giỏ hàng (Khóa chính)

        [Required]
        public string UserId { get; set; } // Mã người dùng (Khóa ngoại)

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; } // Navigation property tới AppUser

        [Required]
        public Guid ProductVariantId { get; set; } // Mã biến thể sản phẩm (Khóa ngoại)

        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; } // Navigation property tới ProductVariant

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Số lượng sản phẩm trong giỏ

        [Required]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow; // Thời gian thêm sản phẩm vào giỏ
    }
}
