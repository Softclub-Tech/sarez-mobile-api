using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    [MaxLength(100)]
    public string ProductName { get; set; } = null!;
    [MaxLength(50)]
    public string Code { get; set; } = null!;
    public int Quantity { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public bool HasDiscountPrice { get; set; }
    public decimal? DiscountPrice { get; set; }
    [MaxLength(50)]
    public string? Weight { get; set; }
    [MaxLength(50)]
    public string? Size { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    public int ColorId { get; set; }
    public Color Color { get; set; } = null!;
    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = null!;
    public int? SmartphoneId { get; set; }
    public Smartphone? Smartphone { get; set; }
    public List<ProductImage> ProductImages { get; set; } = null!;
    public List<Cart> Carts { get; set; } = null!;
}