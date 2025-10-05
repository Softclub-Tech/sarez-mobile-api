namespace Domain.Filters;

public class ProductFilter : PaginationFilter
{
    public string? UserId { get; set; }
    public string? ProductName { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public int BrandId { get; set; }
    public int ColorId { get; set; }
    public int CategoryId { get; set; }
    public int SubcategoryId { get; set; }
}