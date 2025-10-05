using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.CartDTOs;

public class AddProductInCartDto
{
    [Required]
    public int ProductId { get; set; }
}