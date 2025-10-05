using Domain.Dtos.BrandDtos;
using Domain.Dtos.ColorDtos;

namespace Domain.Dtos.ProductDtos;

public class GetProductPageDto
{
    public List<GetProductsDto>? Products { get; set; }
    public List<GetColorDto>? Colors { get; set; }
    public List<GetBrandDto>? Brands { get; set; }
    public GetMinMaxPriceDto? MinMaxPrice { get; set; }
}