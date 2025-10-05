using System.Net;

namespace Domain.Response;

public class Response<T>
{
    public T? Data { get; set; }

    public List<string> Errors { get; set; } = new();

    public int StatusCode { get; set; }

    public Response(T data)
    {
        Data = data;
        StatusCode = 200;
    }

    public Response(HttpStatusCode statusCode, string message)
    {
        StatusCode = (int)statusCode;
        Errors.Add(message);
    }

    public Response(HttpStatusCode statusCode, List<string> messages)
    {
        StatusCode = (int)statusCode;
        Errors.AddRange(messages);
    }
}