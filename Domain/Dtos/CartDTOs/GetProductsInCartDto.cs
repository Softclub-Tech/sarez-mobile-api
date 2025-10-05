using Domain.Dtos.ProductDtos;

namespace Domain.Dtos.CartDTOs;

public class GetProductsInCartDto : CartDto
{
    public GetProductsDto Product { get; set; } = null!;
}