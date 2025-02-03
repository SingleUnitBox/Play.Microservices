using System.Net;
using Play.Common.Abs.Exceptions;
using Play.Common.Exceptions;
using Play.Common.Exceptions.Mappers;


namespace Play.Items.Infra.Exceptions;

public class ItemsExceptionToResponseMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
            PlayException => new ItemsExceptionResponse(new Error(exception.GetType().Name, exception.Message),
                HttpStatusCode.BadRequest),
            _ => new ItemsExceptionResponse(new Error("Play.Items_error", "There was a Play.Items_error."),
                HttpStatusCode.InternalServerError)
        };
}