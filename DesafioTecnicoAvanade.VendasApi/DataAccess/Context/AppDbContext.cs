using DesafioTecnicoAvanade.VendasApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            mb.Entity<CartHeader>().Property(c => c.UserId).HasMaxLength(255).IsRequired();

            mb.Entity<Order>().Property(c => c.UserId).HasMaxLength(255).IsRequired();
            mb.Entity<Order>().Property(c => c.Total).HasPrecision(12, 2);
            mb.Entity<Order>().Property(c => c.OrderDate).IsRequired();
            mb.Entity<Order>().HasMany(c => c.OrderItems).WithOne(c => c.Order).HasForeignKey(c => c.OrderId);

            mb.Entity<OrderItem>().Property(c => c.ProductName).HasMaxLength(100).IsRequired();
            mb.Entity<OrderItem>().Property(c => c.Price).HasPrecision(12, 2);
            mb.Entity<OrderItem>().Property(c => c.Quantity).IsRequired();
        }
    }
}
