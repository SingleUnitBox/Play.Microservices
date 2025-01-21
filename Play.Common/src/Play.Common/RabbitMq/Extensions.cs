using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.RabbitMq;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqClient>(sp =>
            new RabbitMqClient(sp.GetRequiredService<IConfiguration>()));
        
        return services;
    }
}