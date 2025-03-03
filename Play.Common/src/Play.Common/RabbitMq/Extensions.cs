using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Builder;
using Play.Common.RabbitMq.Connection;
using Play.Common.RabbitMq.Consumers;
using Play.Common.RabbitMq.CorrelationContext;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public static class Extensions
{
    public static IRabbitMqBuilder AddRabbitMq(this IServiceCollection services)
    {
        var rabbitSettings = services.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
        services.AddSingleton(rabbitSettings);
        
        services.AddSingleton<IRabbitMqClient>(sp =>
            new RabbitMqClient(sp.GetRequiredService<IConfiguration>()));
        
        services.AddSingleton<IConnection>(sp =>
        {
            var rabbitMqClient = sp.GetRequiredService<IRabbitMqClient>();
            return rabbitMqClient.GetConnection().GetAwaiter().GetResult();
        });
        
        services.AddSingleton<IBusPublisher, BusPublisher>();
        var builder = new RabbitMqBuilder(services);
        services.AddSingleton(builder);
        services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
        return builder;
    }

    public static IRabbitMqBuilder AddConnectionProvider(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<RabbitMqSettings>();
            var serviceName = sp.GetRequiredService<ServiceSettings>().ServiceName;
            var factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                // Port
                // UserName
                // Password
                // VirtualHost
            };
            
            var consumerConnection = factory.CreateConnection($"{serviceName}-consumer");
            var producerConnection = factory.CreateConnection($"{serviceName}-producer");
            var connectionProvider = new ConnectionProvider(consumerConnection, producerConnection);
            
            return connectionProvider;
        });
        
        return builder;
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