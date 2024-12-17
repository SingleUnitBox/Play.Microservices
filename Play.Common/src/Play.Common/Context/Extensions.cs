using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Context;

public static class Extensions
{
    public static IServiceCollection AddContext(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddTransient<IContext>(sp =>
            sp.GetRequiredService<IContextFactory>().Create());

        services.AddScoped<IScopedContext, ScopedContext>();
        
        return services;
    }
}