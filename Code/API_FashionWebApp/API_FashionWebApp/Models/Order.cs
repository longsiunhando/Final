using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FashionWebApp.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } // Mã đơn hàng (Khóa chính)

        [Required]
        public string UserId { get; set; } // Mã người dùng (Khóa ngoại)

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; } // Navigation property tới AppUser

        [Required]
        public DateTime OrderDate { get; set; } // Ngày đặt hàng

        [Required]
        [StringLength(50)]
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Trạng thái đơn hàng (Pending, Shipping, Delivered, Canceled, etc.)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } // Tổng giá trị đơn hàng

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; } // Phí vận chuyển

        [Required]
        [StringLength(100)]
        public PaymentMethod PaymentMethod { get; set; } // Phương thức thanh toán (COD, Credit Card, PayPal, etc.)

        [Required]
        [StringLength(500)]
        public string ShippingAddress { get; set; } // Địa chỉ giao hàng

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo đơn hàng

        public DateTime? UpdatedAt { get; set; } // Ngày cập nhật đơn hàng

        // Navigation property - Liên kết với bảng OrderDetail
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
    public enum OrderStatus
    {
        Pending = 1,
        Shipping = 2,
        Delivered = 3,
        Canceled = 4,
    }
    public enum PaymentMethod
    {
        COD = 1,
        CreditCard = 2,
        MoMo = 3,
        PayPal = 4,
    }
}
