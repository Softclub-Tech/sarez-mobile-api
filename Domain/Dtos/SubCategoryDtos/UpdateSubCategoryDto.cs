using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SubCategoryDtos;

public class UpdateSubCategoryDto : AddSubCategoryDto
{
    [Required]
    public int Id { get; set; }
}