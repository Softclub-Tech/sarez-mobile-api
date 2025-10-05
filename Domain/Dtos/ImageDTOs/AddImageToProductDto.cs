using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.ImageDTOs;

public class AddImageToProductDto
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public List<IFormFile> Files { get; set; } = null!;
}