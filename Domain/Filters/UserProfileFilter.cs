namespace Domain.Filters;

public class UserProfileFilter : PaginationFilter
{
    public string? UserName { get; set; }
}