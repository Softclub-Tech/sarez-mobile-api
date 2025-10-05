using System.Net;

namespace Domain.Response;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
    public int TotalRecord { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize, int totalRecord) : base(data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecord = totalRecord;
        TotalPage = (int)Math.Ceiling(totalRecord / (float)pageSize);
    }

    public PagedResponse(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
    }

    public PagedResponse(HttpStatusCode statusCode, List<string> messages) : base(statusCode, messages)
    {
    }
}