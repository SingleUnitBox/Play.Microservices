using System.Net;
using Play.Common.Exceptions;

namespace Play.Catalog.Infra.Exceptions;

public class CatalogExceptionResponse : ExceptionResponse
{
    public CatalogExceptionResponse(Error error, HttpStatusCode httpStatusCode) 
        : base(error, httpStatusCode)
    {
    }
}