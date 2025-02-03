using System.Net;
using Play.Common.Exceptions;

namespace Play.Items.Infra.Exceptions;

public class ItemsExceptionResponse : ExceptionResponse
{
    public ItemsExceptionResponse(Error error, HttpStatusCode httpStatusCode)
        : base(error, httpStatusCode)
    {
    }
}