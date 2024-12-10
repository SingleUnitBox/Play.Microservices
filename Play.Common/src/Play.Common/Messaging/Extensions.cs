using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Messaging;

namespace Play.Common.Messaging;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<IBusPublisher, BusPublisher>();
        
        return services;
    }
}