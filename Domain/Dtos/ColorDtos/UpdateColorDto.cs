using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.ColorDtos;

public class UpdateColorDto : AddColorDto
{
    [Required]
    public int Id { get; set; }
}