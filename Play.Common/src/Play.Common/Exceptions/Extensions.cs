using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs;
using Play.Common.Abs.Exceptions;
using Play.Common.Exceptions.Mappers;

namespace Play.Common.Exceptions;

public static class Extensions
{
    public static IPlayConfigurator AddExceptionHandling(this IPlayConfigurator playConfigurator)
    {
        playConfigurator.Services.AddSingleton<IExceptionCompositionRootMapper, ExceptionCompositionRootMapper>();
        playConfigurator.Services.AddSingleton<IExceptionToResponseMapper, DefaultExceptionToResponseMapper>();
        playConfigurator.Services.AddScoped<ErrorHandlingMiddleware>();
        
        return playConfigurator;
    }

    public static IServiceCollection AddCustomExceptionToResponseMapper<TMapperImplementation>(this IServiceCollection services)
        where TMapperImplementation : class, IExceptionToResponseMapper
    {
        services.AddSingleton<IExceptionToResponseMapper, TMapperImplementation>();
        
        return services;
    }

    public static IPlayConfigurator AddCustomExceptionToMessageMapper<TMapper>(this IPlayConfigurator playConfigurator)
        where TMapper : class, IExceptionToMessageMapper
    {
        playConfigurator.Services.AddSingleton<IExceptionToMessageMapper, TMapper>();
        
        return playConfigurator;
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        return app;
    }
}