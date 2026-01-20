using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Builder;
using Play.Common.Messaging.Connection;
using Play.Common.Messaging.Consumers;
using Play.Common.Messaging.CorrelationContext;
using Play.Common.Messaging.Deduplication;
using Play.Common.Messaging.Deduplication.Data;
using Play.Common.Messaging.Deduplication.FilterSteps;
using Play.Common.Messaging.Executor;
using Play.Common.Messaging.Ordering;
using Play.Common.Messaging.Ordering.Attributes;
using Play.Common.Messaging.Outbox;
using Play.Common.Messaging.Outbox.Data;
using Play.Common.Messaging.Outbox.FilterSteps;
using Play.Common.Messaging.Resiliency;
using Play.Common.Messaging.Topology;
using Play.Common.PostgresDb;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Common.Messaging;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, 
        IConfiguration configuration,
        Action<IRabbitMqBuilder> builder)
    {
        var rabbitBuilder = new RabbitMqBuilder(services, configuration);
        builder(rabbitBuilder);
        services.AddSingleton(builder);
        
        var rabbitSettings = services.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
        services.AddSingleton(rabbitSettings);
        services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
        services.AddSingleton<MessagePropertiesAccessor>();
        
        var topologySettings = services.GetSettings<TopologySettings>(nameof(TopologySettings));
        services.AddSingleton<TopologyReadinessAccessor>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<TopologyReadinessAccessor>>();
            var accessor = new TopologyReadinessAccessor(logger, topologySettings.Enabled);

            return accessor;
        });
        
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

    public static IRabbitMqBuilder AddBusPublisher(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton<IBusPublisher, RabbitMqBusPublisher>();
        
        return builder;
    }

    public static IRabbitMqBuilder AddEventConsumer(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton<IEventConsumer, EventConsumer>();
        builder.Services.AddHostedService<EventConsumerService>();
        
        return builder;
    }

    public static IRabbitMqBuilder AddCommandConsumer(this IRabbitMqBuilder builder)
    {
        builder.Services.AddSingleton<ICommandConsumer, CommandConsumer>();
        builder.Services.AddHostedService<CommandConsumerService>();

        return builder;
    }

    public static IRabbitMqBuilder AddResiliency(this IRabbitMqBuilder builder)
    {
        var consumer = builder.Configuration
                            .GetSection($"{nameof(ResiliencySettings)}:Consumer")
                            .Get<ConsumerResiliencySettings>() 
                        ?? new ConsumerResiliencySettings(
                            BrokerRetriesEnabled: true,
                            BrokerRetriesLimit: 3,
                            ConsumerRetriesLimit: 3,
                            MaxMessagesFetchedPerConsumer: 10);
        var producer = builder.Configuration
                           .GetSection($"{nameof(ResiliencySettings)}:Producer")
                           .Get<ProducerResiliencySettings>()
                       ?? new ProducerResiliencySettings(PublishMandatoryEnabled: false, PublisherConfirmsEnabled: false);
        var resiliencySettings = new ResiliencySettings(consumer, producer);
        builder.Services.AddSingleton(resiliencySettings);

        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerRetryDecorator<>));
        builder.Services.AddSingleton<ReliablePublishing>();
        builder.Services.AddSingleton<ReliableConsuming>();
        
        return builder;
    }

    public static IRabbitMqBuilder AddTopologyInitializer(this IRabbitMqBuilder builder)
    {
        var topologySettings = builder.Services.GetSettings<TopologySettings>(nameof(TopologySettings));
        if (topologySettings.Enabled)
        {
            builder.Services.AddTransient<ITopologyBuilder, RabbitMqTopologyBuilder>();
            builder.Services.AddSingleton(topologySettings);
            builder.Services.AddHostedService<TopologyInitializer>();
        }
        
        return builder;
    }

    public static IRabbitMqBuilder AddMessageExecutor(this IRabbitMqBuilder builder)
    {
        builder.Services.AddScoped<IMessageExecutor, MessageExecutor>();
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(MessageExecutorCommandHandlerDecorator<>));
        
        return builder;
    }

    public static IRabbitMqBuilder AddDeduplication(this IRabbitMqBuilder builder)
    {
        builder.Services.AddPostgresDb<DeduplicationDbContext>();
        builder.Services.AddScoped<IDeduplicationStore, PostgresDeduplicationStore>();
        builder.Services.AddTransient<IMessageFilterStep, DeduplicationBeforeStep>();
        builder.Services.AddTransient<IMessageFilterStep, DeduplicationWithinStep>();
        
        return builder;
    }

    public static IRabbitMqBuilder AddOutbox(this IRabbitMqBuilder builder)
    {
        var enabled = builder.Services.GetSettings<OutboxSettings>(nameof(OutboxSettings))
            .Enabled;
        if (enabled is false)
        {
            builder.Services.AddPostgresDb<OutboxDbContext>();
            return builder;
        }
        
        var settings = builder.Services.GetSettings<OutboxSettings>(nameof(OutboxSettings));
        builder.Services.AddSingleton(settings);
        builder.Services.AddPostgresDb<OutboxDbContext>();
        builder.Services.AddScoped<IMessageOutbox, PostgresMessageOutbox>();
        // should OutboxMessagePublisher be Singleton?
        builder.Services.AddScoped<IBusPublisher, OutboxMessagePublisher>();

        builder.Services.AddHostedService<OutboxBackgroundService>();
        builder.Services.AddSingleton<OutboxLocalCache>();
        builder.Services.AddSingleton<OutboxPublishChannel>();
        builder.Services.AddTransient<IMessageFilterStep, OutboxBeforeStep>();
        builder.Services.AddTransient<IMessageFilterStep, OutboxAfterStep>();
        
        return builder;
    }

    public static IRabbitMqBuilder AddOutOfOrderDetection(this IRabbitMqBuilder builder)
    {
        var enabled = builder.Services.GetSettings<OutOfOrderDetectionSettings>(nameof(OutOfOrderDetectionSettings))
            .Enabled;
        if (enabled is false)
        {
            return builder;
        }

        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(IgnoreOutOfOrderEventDecorator<>));
        builder.Services.AddSingleton(provider =>
        {
            var factory = (Type type) =>
            {
                var scope = provider.CreateScope();
                return scope.ServiceProvider.GetService(type);
            };

            return new OutOfOrderDetector(factory, provider.GetService<ILogger<OutOfOrderDetector>>()!);
        });

        builder.Services.Scan(x => x.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IGetMessageRelatedEntityVersion<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

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