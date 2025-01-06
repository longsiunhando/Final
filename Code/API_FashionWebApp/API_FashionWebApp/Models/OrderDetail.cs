using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FashionWebApp.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; } // Mã chi tiết đơn hàng (Khóa chính)

        [Required]
        public Guid OrderId { get; set; } // Mã đơn hàng (Khóa ngoại)

        [ForeignKey("OrderId")]
        public Order Order { get; set; } // Navigation property tới Order

        [Required]
        public Guid ProductVariantId { get; set; } // Mã biến thể sản phẩm (Khóa ngoại)

        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; } // Navigation property tới ProductVariant

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Số lượng sản phẩm

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá sản phẩm tại thời điểm đặt hàng

        [NotMapped]
        public decimal TotalPrice => Quantity * Price; // Tổng giá trị sản phẩm trong đơn hàng
    }
}
