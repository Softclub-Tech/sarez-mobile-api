using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.BrandDtos;

public class BrandDto
{
    [Required] public string BrandName { get; set; } = null!;
}