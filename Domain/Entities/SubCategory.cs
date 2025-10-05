using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index("SubCategoryName", "CategoryId", IsUnique = true)]
public class SubCategory
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string SubCategoryName { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public List<Smartphone> Smartphones { get; set; } = null!;
    public List<Television> Televisions { get; set; } = null!;
    public List<Product> Products { get; set; } = null!;
}