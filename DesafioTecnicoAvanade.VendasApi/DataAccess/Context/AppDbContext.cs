using DesafioTecnicoAvanade.VendasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product>? Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Product>().HasKey(c => c.Id);

            mb.Entity<Product>().Property(c => c.Id).ValueGeneratedNever();

            mb.Entity<Product>().Property(c => c.Name).HasMaxLength(100).IsRequired();

            mb.Entity<Product>().Property(c => c.Description).HasMaxLength(255).IsRequired();

            mb.Entity<Product>().Property(c => c.CategoryName).HasMaxLength(100).IsRequired();

            mb.Entity<Product>().Property(c => c.Price).HasPrecision(12, 2);

            //CartHeader
            mb.Entity<CartHeader>().Property(c => c.UserId).HasMaxLength(255).IsRequired();
        }
    }
}
