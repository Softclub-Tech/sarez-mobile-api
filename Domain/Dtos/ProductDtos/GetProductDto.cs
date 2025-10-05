using Domain.Dtos.CartDTOs;
using Domain.Dtos.ImageDTOs;
using Domain.Dtos.UserProfileDtos;

namespace Domain.Dtos.ProductDtos;

public class GetProductDto : ProductDto
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Brand { get; set; } = null!;
    public int ColorId { get; set; }
    public string Color { get; set; } = null!;
    public bool ProductInMyCart { get; set; }
    public List<GetImageDto> Images { get; set; } = [];
    public List<GetUserShortInfoDto>? Users { get; set; }
    public CartDto? ProductInfoFromCart { get; set; }
}