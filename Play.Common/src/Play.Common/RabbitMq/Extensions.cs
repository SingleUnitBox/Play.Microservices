using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Builder;
using Play.Common.RabbitMq.Connection;
using Play.Common.RabbitMq.Consumers;
using Play.Common.RabbitMq.CorrelationContext;
using Play.Common.RabbitMq.Topology;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, Action<IRabbitMqBuilder> builder)
    {
        var rabbitBuilder = new RabbitMqBuilder(services);
        builder(rabbitBuilder);
        services.AddSingleton(builder);
        
        var rabbitSettings = services.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
        services.AddSingleton(rabbitSettings);
        services.AddTransient<ITopologyBuilder, RabbitMqTopologyBuilder>();
        var topologySettings = services.GetSettings<TopologySettings>(nameof(TopologySettings));
        services.AddSingleton(topologySettings);
        services.AddSingleton<IBusPublisher, BusPublisher>();
        services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
        
        return services;
    }

    public static IRabbitMqBuilder AddConnectionProvider(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton<ConnectionProvider>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<ConnectionProvider>>();
            var settings = sp.GetRequiredService<RabbitMqSettings>();
            var serviceName = sp.GetRequiredService<ServiceSettings>().ServiceName;
            var factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
                VirtualHost = settings.VirtualHost
            };
            logger.LogInformation($"Connecting to RabbitMQ at {settings.Host}:{settings.Port}");

            try
            {
                var consumerConnection = factory.CreateConnection($"{serviceName}-consumer");
                var producerConnection = factory.CreateConnection($"{serviceName}-producer");
                
                logger.LogInformation("RabbitMQ connections created successfully!");
                return new ConnectionProvider(consumerConnection, producerConnection);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to create RabbitMQ connections", e);
            }
        });
        
        return builder;
    }

    public static IRabbitMqBuilder AddChannelFactory(this IRabbitMqBuilder builder)
    {
        builder.Services.AddTransient<ChannelFactory>();
        
        return builder;
    }
    
    public static IRabbitMqBuilder AddEventConsumer(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton<IEventConsumer, EventConsumer>();
        // Services.AddHostedService<EventConsumerService>();
        
        return builder;
    }

    public static IRabbitMqBuilder AddCommandConsumer(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton<ICommandConsumer, CommandConsumer>();
        builder.Services.AddHostedService<CommandConsumerService>();

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