using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace Play.Catalog.Infra.Exceptions;

public class CatalogExceptionMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
            PlayException => new CatalogExceptionResponse(new JSType.Error(exception.GetType().Name, exception.Message),
                HttpStatusCode.BadRequest),
            _ => new CatalogExceptionResponse(new JSType.Error("catalog_error", "There was a catalog_error."), 
                HttpStatusCode.InternalServerError)
        };
}