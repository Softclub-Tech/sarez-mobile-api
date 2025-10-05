using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.TelevisionDtos;

public class AddTelevisionDto : TelevisionDto
{
    [Required]
    public int SubCategoryId { get; set; }
}