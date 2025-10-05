using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.CategoryDtos;

public class AddCategoryDto : CategoryDto
{
    [Required]
    public IFormFile CategoryImage { get; set; } = null!;
}