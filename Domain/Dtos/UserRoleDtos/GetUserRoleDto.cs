using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.UserRoleDtos;

public class GetUserRoleDto
{
    [Required]
    public string Id { get; set; } = null!;
    public string? Name { get; set; }
}