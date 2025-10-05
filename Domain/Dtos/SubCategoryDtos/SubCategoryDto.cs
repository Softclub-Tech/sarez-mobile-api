using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SubCategoryDtos;

public class SubCategoryDto
{
    [Required]
    public string SubCategoryName { get; set; } = null!;
}