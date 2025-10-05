using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.BrandDtos;

public class UpdateBrandDto : AddBrandDto
{
    [Required]
    public int Id { get; set; }
}