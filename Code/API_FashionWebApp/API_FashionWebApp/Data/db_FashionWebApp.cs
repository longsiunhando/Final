using API_FashionWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_FashionWebApp.Data
{
    public class db_FashionWebApp:IdentityDbContext<AppUser>
    {
        public db_FashionWebApp(DbContextOptions<db_FashionWebApp> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ giữa User và Role
            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Cấu hình mối quan hệ giữa Product và ProductVariant
            modelBuilder.Entity<ProductVariant>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa biến thể khi sản phẩm bị xóa

            // Cấu hình mối quan hệ giữa Cart và AppUser (1 User có nhiều Cart)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Carts) // Một AppUser có thể có nhiều Cart
                .HasForeignKey(c => c.UserId) // Cart có UserId làm khóa ngoại
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa Order và OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa chi tiết đơn hàng khi đơn hàng bị xóa

            // Cấu hình chỉ mục cho Product
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name")
                .IsUnique();

            // Cấu hình chỉ mục cho Order
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderDate)
                .HasDatabaseName("IX_Order_Date");

            // Cấu hình chỉ mục cho Cart
            modelBuilder.Entity<Cart>()
                .HasIndex(c => new { c.UserId, c.ProductVariantId })
                .HasDatabaseName("IX_Cart_User_ProductVariant");
        }
    }
}
