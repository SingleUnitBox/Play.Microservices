using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Exceptions.Mappers;

namespace Play.Common.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionCompositionRootMapper, ExceptionCompositionRootMapper>();
        services.AddSingleton<IExceptionToResponseMapper, DefaultExceptionToResponseMapper>();
        services.AddScoped<ErrorHandlingMiddleware>();
        
        return services;
    }

    public static IServiceCollection AddCustomExceptionToResponseMapper<TMapperImplementation>(this IServiceCollection services)
        where TMapperImplementation : class, IExceptionToResponseMapper
    {
        services.AddSingleton<IExceptionToResponseMapper, TMapperImplementation>();
        
        return services;
    }

    public static IServiceCollection AddCustomExceptionToMessageMapper<TMapper>(this IServiceCollection services)
        where TMapper : class, IExceptionToMessageMapper
    {
        services.AddSingleton<IExceptionToMessageMapper, TMapper>();
        
        return services;
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        return app;
    }
}