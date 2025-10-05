using Domain.Dtos.UserRoleDtos;

namespace Domain.Dtos.UserProfileDtos;

public class GetUserProfileDto : UserProfileDto
{
    public string? UserName { get; set; }
    public string UserId { get; set; } = null!;
    public string? Image { get; set; }
    public List<GetUserRoleDto> UserRoles { get; set; } = [];
}