using Microsoft.Extensions.DependencyInjection;
using Play.Common.RabbitMq;

namespace Play.Items.Infra.Consumers;

public static class Extensions
{
    public static IServiceCollection AddRabbitMqConsumers(this IServiceCollection services)
    {
        services.AddRabbitMq();
            //.WithEventConsumer();
        services.AddSingleton<CommandConsumer>();
        services.AddHostedService<CommandConsumerService>();
        
        return services;
    }
}