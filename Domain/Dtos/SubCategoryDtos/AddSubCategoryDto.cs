using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SubCategoryDtos;

public class AddSubCategoryDto : SubCategoryDto
{
    [Required]
    public int CategoryId { get; set; }
}