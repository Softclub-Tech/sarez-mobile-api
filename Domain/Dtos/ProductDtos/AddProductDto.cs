using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.ProductDtos;

public class AddProductDto : ProductDto
{
    [Required] public List<IFormFile> Images { get; set; } = null!;
    [Required]
    public int BrandId { get; set; }
    [Required]
    public int ColorId { get; set; }
}