using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.AccountDtos;

public class RegisterDto
{
    [Required] public string UserName { get; set; } = null!;
    [Required, DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required, Compare("Password"), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}