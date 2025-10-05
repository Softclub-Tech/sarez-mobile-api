namespace Domain.Filters;

public class BrandFilter : PaginationFilter
{
    public string? BrandName { get; set; }
    public int BrandId { get; set; }
}