using System.Net;
using Humanizer;
using Play.Common.Abs.Exceptions;

namespace Play.Common.Exceptions;

public class DefaultExceptionToResponseMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
            PlayException playException => new ExceptionResponse(
            new Error(GetErrorCode(playException), playException.Message),
                HttpStatusCode.BadRequest),
            _ => new ExceptionResponse(new Error("error", "There was an error."),
                HttpStatusCode.InternalServerError)
        };
    
    private static string GetErrorCode(Exception exception)
        => exception.GetType().Name.Underscore().Replace("_exception", string.Empty);
}