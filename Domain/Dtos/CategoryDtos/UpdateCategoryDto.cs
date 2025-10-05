using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.CategoryDtos;

public class UpdateCategoryDto : AddCategoryDto
{
    [Required]
    public int Id { get; set; }
}