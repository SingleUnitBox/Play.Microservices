using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqClient>(sp =>
            new RabbitMqClient(sp.GetRequiredService<IConfiguration>()));
        services.AddSingleton<IConnection>(sp =>
        {
            var rabbitMqClient = sp.GetRequiredService<IRabbitMqClient>();
            return rabbitMqClient.GetConnection().GetAwaiter().GetResult();
        });
        services.AddSingleton<IBusPublisher, BusPublisher>();
        return services;
    }

    public static IServiceCollection WithEventConsumer(this IServiceCollection services)
    {
        services.AddSingleton<IEventConsumer, EventConsumer>();
        services.AddHostedService<EventConsumerService>();
        
        return services;
    }

    public static string GetExchangeName<TMessage>(this TMessage message)
        => message.GetType().GetExchangeName();

    public static string GetExchangeName(this Type messageType)
        => $"{messageType.Name.Underscore()}_exchange";
    
    public static string GetQueueName<TMessage>(this TMessage message)
        => message.GetType().GetQueueName();

    public static string GetQueueName(this Type messageType)
        => $"{messageType.FullName.Underscore()}_queue";

    public static string GetRoutingKey<TMessage>(this TMessage message)
        => message.GetType().GetRoutingKey();
    
    public static string GetRoutingKey(this Type message)
        => $"{message.Name.Underscore()}";
}