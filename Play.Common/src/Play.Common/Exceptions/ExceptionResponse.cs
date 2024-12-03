using System.Net;

namespace Play.Common.Exceptions;

public class ExceptionResponse
{
    public Error Error { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    
    public ExceptionResponse(Error error, HttpStatusCode httpStatusCode)
    {
        Error = error;
        HttpStatusCode = httpStatusCode;
    }
}