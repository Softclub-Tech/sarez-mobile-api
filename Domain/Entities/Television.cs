using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Television
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [MaxLength(100)]
    public string Model { get; set; } = null!;
    public int? NumberOfSpeaker { get; set; }
    [MaxLength(50)]
    public string? Diagonal { get; set; }
    [MaxLength(50)]
    public string? DisplayResolution { get; set; }
}