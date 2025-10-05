using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.UserProfileDtos;

public class AddUserProfileDto : UserProfileDto
{
    [Required]
    public IFormFile? Image { get; set; }
}