namespace Domain.Dtos.SubCategoryDtos;

public class GetSubCategoryDto : SubCategoryDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
}