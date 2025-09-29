using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common;
using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.AppInitializer;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.Messaging;
using Play.Common.Observability;
using Play.Common.PostgresDb;
using Play.Common.Queries;
using Play.Common.Serialization;
using Play.Common.Settings;
using Play.Items.Application.Services;
using Play.Items.Infra.Exceptions;
using Play.Items.Infra.Metrics;
using Play.Items.Infra.Postgres;
using Play.Items.Infra.Postgres.Repositories;
using Play.Items.Infra.Services;

namespace Play.Items.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddHostedService<AppInitializer>();
        services.AddContext();
        // services.AddHostedService<CreateItemConsumerService>();
        // services.AddHostedService<NonGenericCommandConsumerService>();
        // services.AddScoped<ItemChangesHandler>();

        services.AddPostgresDb<ItemsPostgresDbContext>();
        services.AddPostgresRepositories();
        //caching
        //builder.Services.AddScoped<IItemRepository, CachedItemRepository>();
        //builder.Services.AddMemoryCache();
        //builder.Services.AddCaching();
        
        services.AddCommands();
        services.AddLoggingCommandHandlerDecorator();
        services.AddQueries();
        services.AddLoggingQueryHandlerDecorator();
        services.AddEvents();
        services.AddLoggingEventHandlerDecorator();
        
        services.AddSerialization();
        services.AddScoped<IMessageBroker, MessageBroker>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddSingleton<IEventMapper, EventMapper>();
        services.Scan(a => a.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<ItemsMetrics>();
        services.AddScoped<CustomMetricsMiddleware>();

        services.AddRabbitMq(rabbitBuilder =>
            rabbitBuilder
                .AddChannelFactory()
                .AddConnectionProvider()
                .AddCommandConsumer()
                .AddEventConsumer()
                .AddDeduplication());
                //.AddTopologyInitializer());
                
                
        services.AddPlayMicroservice(
            configuration,
            config =>
            {
                config.AddExceptionHandling();
                config.AddCustomExceptionToMessageMapper<ExceptionToMessageMapper>();
                config.AddSettings<ServiceSettings>(nameof(ServiceSettings));
                config.AddPlayMetrics(["play.items.meter"]);
                config.AddPlayTracing(environment);
            });
        
        return services;
    }
}