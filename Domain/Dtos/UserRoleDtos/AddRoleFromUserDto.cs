using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.UserRoleDtos;

public class AddRoleFromUserDto
{
    [Required]
    public string UserId { get; set; } = null!;
    [Required]
    public string RoleId { get; set; } = null!;
}