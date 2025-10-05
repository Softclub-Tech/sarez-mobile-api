using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index("BrandName", IsUnique = true)]
public class Brand
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string BrandName { get; set; } = null!;
    public List<Product> Products { get; set; } = null!;
}