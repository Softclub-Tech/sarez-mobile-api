using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index("ApplicationUserId", "ProductId", IsUnique = true)]
public class Cart
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}