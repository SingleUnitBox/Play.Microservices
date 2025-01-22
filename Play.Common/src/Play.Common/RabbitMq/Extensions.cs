using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.RabbitMq;

namespace Play.Common.RabbitMq;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqClient>(sp =>
            new RabbitMqClient(sp.GetRequiredService<IConfiguration>()));
        services.AddSingleton<IBusPublisher, BusPublisher>();
        services.AddSingleton<IEventConsumer, EventConsumer>();
        services.AddHostedService<EventConsumerService>();
        
        return services;
    }

    public static string GetExchangeName<TMessage>(this TMessage message)
        => typeof(TMessage).GetExchangeName();

    public static string GetExchangeName(this Type messageType)
        => $"{messageType.Name.Underscore()}_exchange";
    
    public static string GetQueueName<TMessage>(this TMessage message)
        => typeof(TMessage).GetQueueName();

    public static string GetQueueName(this Type messageType)
        => $"{messageType.Name.Underscore()}_queue";
    
    public static string GetRoutingKey<TMessage>(this TMessage message)
        => message.GetType().Name.Underscore();
}