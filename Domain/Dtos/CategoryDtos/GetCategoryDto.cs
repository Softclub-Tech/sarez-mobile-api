using Domain.Dtos.SubCategoryDtos;

namespace Domain.Dtos.CategoryDtos;

public class GetCategoryDto : CategoryDto
{
    public int Id { get; set; }
    public string CategoryImage { get; set; } = null!;
    public List<GetSubCategoryDto> SubCategories { get; set; } = null!;
}