using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.TelevisionDtos;

public class UpdateTelevisionDto : AddTelevisionDto
{
    [Required]
    public int Id { get; set; }
}