using Domain.Dtos.CartDTOs;

namespace Domain.Dtos.ProductDtos;

public class GetProductsDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Price { get; set; }
    public bool HasDiscount { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Quantity { get; set; }
    public bool ProductInMyCart { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public CartDto? ProductInfoFromCart { get; set; }
}