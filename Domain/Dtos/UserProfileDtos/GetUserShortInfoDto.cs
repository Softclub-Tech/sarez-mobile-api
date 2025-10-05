namespace Domain.Dtos.UserProfileDtos;

public class GetUserShortInfoDto
{
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string ImageName { get; set; } = null!;
}