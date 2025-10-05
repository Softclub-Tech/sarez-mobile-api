using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public List<Product> Products { get; set; } = null!;
    public UserProfile UserProfile { get; set; } = null!;
    public List<Cart> Carts { get; set; } = null!;
}