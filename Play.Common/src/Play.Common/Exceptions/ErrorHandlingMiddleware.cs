using Microsoft.AspNetCore.Http;
using Play.Common.Abs.Exceptions;
using Play.Common.Context;
using Play.Common.Exceptions.Mappers;

namespace Play.Common.Exceptions;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly IExceptionCompositionRootMapper _exceptionCompositionRootMapper;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    private readonly IScopedContext _scopedContext;

    public ErrorHandlingMiddleware(IExceptionCompositionRootMapper exceptionCompositionRootMapper,
        IScopedContext scopedContext,
        IExceptionToMessageMapper exceptionToMessageMapper = null)
    {
        _exceptionCompositionRootMapper = exceptionCompositionRootMapper;
        _scopedContext = scopedContext;
        _exceptionToMessageMapper = exceptionToMessageMapper;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (_exceptionToMessageMapper != null)
            {
                var rejectedMessage = _exceptionToMessageMapper.Map(exception, _scopedContext.CurrentMessage);
            }

            var response = _exceptionCompositionRootMapper.Map(exception);
            context.Response.StatusCode = (int)response.HttpStatusCode;
            await context.Response.WriteAsJsonAsync(response.Error);
        }
    }
}