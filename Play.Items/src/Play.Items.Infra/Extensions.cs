using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common;
using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Exceptions;
using Play.Common.Observability;
using Play.Common.RabbitMq;
using Play.Common.RabbitMq.Topology;
using Play.Common.Serialization;
using Play.Common.Settings;
using Play.Items.Application.Services;
using Play.Items.Infra.Exceptions;
using Play.Items.Infra.Messaging.Topology;
using Play.Items.Infra.Metrics;
using Play.Items.Infra.Services;

namespace Play.Items.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddHostedService<CreateItemConsumerService>();
        // services.AddHostedService<NonGenericCommandConsumerService>();
        // services.AddScoped<ItemChangesHandler>();

        services.AddSerialization();
        services.AddScoped<IMessageBroker, MessageBroker>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddSingleton<IEventMapper, EventMapper>();
        services.Scan(a => a.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddSingleton<ItemsMetrics>();
        services.AddSingleton<CustomMetricsMiddleware>();
        services.AddPlayMicroservice(
            configuration,
            config =>
            {
                config.AddExceptionHandling();
                config.AddCustomExceptionToMessageMapper<ExceptionToMessageMapper>();
                config.AddSettings<ServiceSettings>(nameof(ServiceSettings));
                config.AddPlayMetrics(["play.items.meter"]);
            });
        
        services.AddRabbitMq(rabbitBuilder =>
            rabbitBuilder
                .AddCommandConsumer()
                .AddEventConsumer()
                .AddConnectionProvider()
                .AddChannelFactory());
        var topologySettings = services.GetSettings<TopologySettings>(nameof(TopologySettings));
        if (topologySettings.Enabled)
        {
            services.AddHostedService<TopologyInitializer>();
        }
        
        return services;
    }
}