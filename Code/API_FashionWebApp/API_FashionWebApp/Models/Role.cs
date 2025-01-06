using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API_FashionWebApp.Models
{
    public class Role:IdentityRole<Guid>
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
