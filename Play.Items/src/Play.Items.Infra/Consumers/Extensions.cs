using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Items.Infra.Consumers;

namespace Play.APIGateway.Commands;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMqClient>(sp =>
            new RabbitMqClient(sp.GetRequiredService<IConfiguration>()));

        services.AddSingleton<CommandConsumer>();
        services.AddHostedService<CommandConsumerService>();
        
        return services;
    }
}