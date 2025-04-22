using Microsoft.EntityFrameworkCore;
using ShopPlatform.Server.Entities;

namespace ShopPlatform.Server.Context
{
    public class BlazorShopDbContext : DbContext
    {
        // Yapıcı metod
        public BlazorShopDbContext(DbContextOptions<BlazorShopDbContext> options)
            : base(options)
        {
        }

        // Veritabanı bağlantısı burada yapılacak
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost;Database=BlazorShopDb;Trusted_Connection=True;TrustServerCertificate=True;");
        //    }
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Bu kod artık gerekli değil çünkü Program.cs içinde DbContext ayarlanıyor
        }


        // DbSet'ler
        public DbSet<Users> Users { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
    }
}
