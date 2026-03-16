using System.Net;

namespace WebApplication1.Data.Exceptions;


/// <summary>
/// <para>Base class for other exceptions to extend.</para>
/// </summary>
public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

    public string? ResponseMessage { get; set; }

    public object? ResponseDetail { get; set; }

    protected ApiException(string message) : base(message) { }

    protected ApiException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    protected ApiException(string message, string responseMessage) : base(message)
    {
        ResponseMessage = responseMessage;
    }

    protected ApiException(string message, HttpStatusCode statusCode, string responseMessage) : base(message)
    {
        StatusCode = statusCode;
        ResponseMessage = responseMessage;
    }

}
