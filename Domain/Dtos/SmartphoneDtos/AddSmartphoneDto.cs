using System.ComponentModel.DataAnnotations;
using Domain.Dtos.ProductDtos;

namespace Domain.Dtos.SmartphoneDtos;

public class AddSmartphoneDto : SmartphoneDto
{
    [Required]
    public AddProductDto AddProductDto { get; set; } = null!;
}