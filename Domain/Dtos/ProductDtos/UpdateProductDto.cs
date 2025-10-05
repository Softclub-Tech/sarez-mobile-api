using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.ProductDtos;

public class UpdateProductDto : ProductDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int BrandId { get; set; }
    [Required]
    public int ColorId { get; set; }
}