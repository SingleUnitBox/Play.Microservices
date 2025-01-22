using Microsoft.Extensions.DependencyInjection;
using Play.Common.RabbitMq;
using Play.Items.Infra.Consumers;

namespace Play.APIGateway.Commands;

public static class Extensions
{
    public static IServiceCollection AddRabbitMqConsumers(this IServiceCollection services)
    {
        services.AddRabbitMq();
        services.AddSingleton<CommandConsumer>();
        services.AddHostedService<CommandConsumerService>();
        services.AddSingleton<IEventConsumer, EventConsumer>();
        services.AddHostedService<EventConsumerService>();
        
        return services;
    }
}