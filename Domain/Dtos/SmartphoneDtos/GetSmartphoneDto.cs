using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SmartphoneDtos;

public class GetSmartphoneDto : SmartphoneDto
{
    public int Id { get; set; }
    [Required]
    public string SubCategory { get; set; } = null!;
}