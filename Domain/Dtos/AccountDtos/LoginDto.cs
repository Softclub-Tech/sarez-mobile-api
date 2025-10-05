using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.AccountDtos;

public class LoginDto
{
    [Required]
    public string UserName { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}