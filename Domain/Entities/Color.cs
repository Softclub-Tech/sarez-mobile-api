using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index("ColorName", IsUnique = true)]
public class Color
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string ColorName { get; set; } = null!;
    public List<Product> Products { get; set; } = null!;
}