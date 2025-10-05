using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationContext : IdentityDbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Color> Colors { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Smartphone> Smartphones { get; set; } = null!;
    public DbSet<SubCategory> SubCategories { get; set; } = null!;
    public DbSet<Television> Televisions { get; set; } = null!;
    public new DbSet<ApplicationUser> Users { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .ToTable(p => p.HasCheckConstraint("Products", @" ""Price"" >= 0"))
            .ToTable(p => p.HasCheckConstraint("Products", @"""DiscountPrice"" >= 0"));

        builder.Entity<Cart>()
            .ToTable(c => c.HasCheckConstraint("Carts", @"""Quantity"" >= 0"));
        
        base.OnModelCreating(builder);
    }
}