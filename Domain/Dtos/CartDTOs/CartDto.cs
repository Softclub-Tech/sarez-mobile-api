using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.CartDTOs;

public class CartDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}