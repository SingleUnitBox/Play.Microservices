using Microsoft.AspNetCore.Http;

namespace Play.Common.Exceptions;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly IExceptionCompositionRootMapper _exceptionCompositionRootMapper;

    public ErrorHandlingMiddleware(IExceptionCompositionRootMapper exceptionCompositionRootMapper)
    {
        _exceptionCompositionRootMapper = exceptionCompositionRootMapper;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var response = _exceptionCompositionRootMapper.Map(exception);
            context.Response.StatusCode = (int)response.HttpStatusCode;
            await context.Response.WriteAsJsonAsync(response.Error);
        }
    }
}