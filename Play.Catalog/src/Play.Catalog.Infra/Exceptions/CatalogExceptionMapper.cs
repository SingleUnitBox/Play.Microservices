using System.Net;
using Play.Common.Exceptions;
using Play.Common.Temp.Exceptions;

namespace Play.Catalog.Infra.Exceptions;

public class CatalogExceptionMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
            PlayException => new CatalogExceptionResponse(new Error(exception.GetType().Name, exception.Message),
                HttpStatusCode.BadRequest),
            _ => new CatalogExceptionResponse(new Error("catalog_error", "There was a catalog_error."), 
                HttpStatusCode.InternalServerError)
        };
}