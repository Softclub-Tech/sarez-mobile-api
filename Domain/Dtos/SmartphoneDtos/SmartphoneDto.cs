using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SmartphoneDtos;

public class SmartphoneDto
{
    [Required]
    public string Model { get; set; } = null!;
    [Required]
    public string Os { get; set; } = null!;
    public string? SimCard { get; set; }
    public string? Communication { get; set; }
    public string? DisplayResolution { get; set; }
    public string? PixelPerInch { get; set; }
    public string? AspectRatio { get; set; }
    public string? ScreenRefreshRate { get; set; }
    public string? DisplayType { get; set; }
    public string? Diagonal { get; set; }
    public string? Processor { get; set; }
    public string? VideoProcessor { get; set; }
    public string? ProcessorFrequency { get; set; }
    public string? NumberOfCores { get; set; }
    public string? Ram { get; set; }
    public string? Rom { get; set; }
}