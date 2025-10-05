using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class UserProfile
{
    [Key, ForeignKey("ApplicationUser"), MaxLength(50)] 
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    [MaxLength(50)] public string FirstName { get; set; } = null!;
    [MaxLength(50)] public string LastName { get; set; } = null!;
    [MaxLength(50)] public string Email { get; set; } = null!;

    [MaxLength(50)] public string PhoneNumber { get; set; } = null!;
    [MaxLength(100)] public string Image { get; set; } = null!;
    public DateOnly Dob { get; set; }
}