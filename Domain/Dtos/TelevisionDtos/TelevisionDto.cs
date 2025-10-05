using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.TelevisionDtos;

public class TelevisionDto
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public string Model { get; set; } = null!;
    public int? NumberOfSpeaker { get; set; }
    public string? Diagonal { get; set; }
    public string? DisplayResolution { get; set; }
}