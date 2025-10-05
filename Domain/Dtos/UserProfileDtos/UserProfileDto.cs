using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.UserProfileDtos;

public class UserProfileDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [Required, DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;
    [Required, DataType(DataType.Date)]
    public DateOnly Dob { get; set; }
}