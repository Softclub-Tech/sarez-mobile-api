using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.CategoryDtos;

public class CategoryDto
{
    [Required]
    public string CategoryName { get; set; } = null!;
}