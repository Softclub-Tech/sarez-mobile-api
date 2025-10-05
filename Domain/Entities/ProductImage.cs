using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [MaxLength(100)]
    public string ImageName { get; set; } = null!;
}