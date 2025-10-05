using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index("CategoryName", IsUnique = true)]
public class Category
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string CategoryName { get; set; } = null!;
    [MaxLength(100)]
    public string CategoryImage { get; set; } = null!;
    public List<SubCategory> SubCategories { get; set; } = null!;
}