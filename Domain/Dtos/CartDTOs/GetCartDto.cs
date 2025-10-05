namespace Domain.Dtos.CartDTOs;

public class GetCartDto
{
    public List<GetProductsInCartDto>? ProductsInCart { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? TotalDiscountPrice { get; set; }
}