using DesafioTecnicoAvanade.EstoqueApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products {  get; set; }
    public DbSet<Category> Categories {  get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Product>().Property(c => c.Name).HasMaxLength(100).IsRequired();

        mb.Entity<Product>().Property(c => c.Description).HasMaxLength(300).IsRequired();

        mb.Entity<Product>().Property(c => c.Price).HasPrecision(12, 2);

        mb.Entity<Category>().HasKey(c => c.Id);

        mb.Entity<Category>().Property(c => c.Name).HasMaxLength(100).IsRequired();

        mb.Entity<Category>().HasMany(g => g.Products).WithOne(c => c.Category).IsRequired().OnDelete(DeleteBehavior.Cascade);

    }

}
