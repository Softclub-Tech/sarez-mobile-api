using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SmartphoneDtos;

public class UpdateSmartphoneDto : AddSmartphoneDto
{
    [Required]
    public int Id { get; set; }
}