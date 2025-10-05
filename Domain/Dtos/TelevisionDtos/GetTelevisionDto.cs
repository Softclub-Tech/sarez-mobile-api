using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.TelevisionDtos;

public class GetTelevisionDto : TelevisionDto
{
    public int Id { get; set; }
    [Required]
    public string SubCategory { get; set; } = null!;
}