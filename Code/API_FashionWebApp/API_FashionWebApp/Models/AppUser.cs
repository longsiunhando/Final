using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API_FashionWebApp.Models
{
    public class AppUser: IdentityUser
    {
        public ICollection<Cart> Carts { get; set; } // Một người dùng có thể có nhiều giỏ hàng

    }
}
