using BPROJESİ.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
  // ApplicationUser sınıfının namespace'i

namespace BPROJESİ.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
     

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<PetAd> PetAds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PetAd>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CartItem>()
                .Property(c => c.ProductPrice)
                .HasColumnType("decimal(18,2)");

            // ApplicationUser kolon eşlemesi:
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasColumnName("Ad");
                entity.Property(e => e.LastName).HasColumnName("Soyad");
                entity.Property(e => e.BirthDate).HasColumnName("BirthDate");
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
