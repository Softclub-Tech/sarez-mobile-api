using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.ColorDtos;

public class ColorDto
{
    [Required]
    public string ColorName { get; set; } = null!;
}