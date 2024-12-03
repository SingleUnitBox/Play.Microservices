using Microsoft.AspNetCore.Http;

namespace Play.Common.Exceptions;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly IExceptionToResponseMapper _exceptionToResponseMapper;

    public ErrorHandlingMiddleware(IExceptionToResponseMapper exceptionToResponseMapper)
    {
        _exceptionToResponseMapper = exceptionToResponseMapper;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var response = _exceptionToResponseMapper.Map(exception);
            context.Response.StatusCode = (int)response.HttpStatusCode;
            await context.Response.WriteAsJsonAsync(response.Error);
        }
    }
}