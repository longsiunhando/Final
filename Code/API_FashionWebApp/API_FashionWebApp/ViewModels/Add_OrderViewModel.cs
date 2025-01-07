using Microsoft.Extensions.Diagnostics.HealthChecks;
using API_FashionWebApp.Models;

namespace API_FashionWebApp.ViewModels
{
    public class Add_OrderViewModel
    {
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        //public OrderStatus Status { get; set; }
        //public decimal TotalPrice { get; set; }
        public decimal ShippingFee { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
    }
}
