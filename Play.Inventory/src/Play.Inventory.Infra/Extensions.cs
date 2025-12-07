using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common;
using Play.Common.Abs.Commands;
using Play.Common.AppInitializer;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Exceptions;
using Play.Common.Http;
using Play.Common.Logging;
using Play.Common.Messaging;
using Play.Common.Observability;
using Play.Common.PostgresDb;
using Play.Common.PostgresDb.UnitOfWork.Decorators;
using Play.Common.Queries;
using Play.Common.Serialization;
using Play.Common.Settings;
using Play.Inventory.Application.Services.Clients;
using Play.Inventory.Infra.Events.External;
using Play.Inventory.Infra.Exceptions;
using Play.Inventory.Infra.Messaging;
using Play.Inventory.Infra.Postgres;
using Play.Inventory.Infra.Postgres.Repositories;
using Play.Inventory.Infra.Postgres.UnitOfWork;
using Play.Inventory.Infra.Services.Clients;

namespace Play.Inventory.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddHostedService<AppInitializer>();
        services.AddContext();
        services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        
        var httpClientSettings = services.GetSettings<HttpClientSettings>(nameof(HttpClientSettings));
        services.AddSingleton(httpClientSettings);
        services.AddHttpClient();
        services.AddTransient<IUserServiceClient, UserServiceClient>();
        services.AddCommonHttpClient();

        //builder.Services.AddMongoDb(builder.Configuration);
        //builder.Services.AddMongoRepositories();
        services.AddPostgresDb<InventoryPostgresDbContext>();
        services.AddPostgresRepositories();
        services.AddAPostgresCommandHandlerDecorator();
        services.AddPostgresUnitOfWork<IInventoryUnitOfWork, InventoryPostgresUnitOfWork>();
        
        services.AddCommands();
        services.AddLoggingCommandHandlerDecorator();
        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.AddQueries();
        services.AddLoggingQueryHandlerDecorator();
        services.AddEvents();
        services.AddLoggingEventHandlerDecorator();
        
        services.AddSerialization();
        
        services.AddRabbitMq(configuration, builder =>
        {
            builder
                .AddConnectionProvider()
                .AddChannelFactory()
                .AddBusPublisher()
                .AddCommandConsumer()
                .AddEventConsumer()
                .AddResiliency();

            //.AddTopologyInitializer();
        });
        
        services.AddPlayMicroservice(configuration,
            config =>
            {
                config.AddExceptionHandling();
                config.AddCustomExceptionToMessageMapper<InventoryExceptionToMessageMapper>();
                config.AddSettings<ServiceSettings>(nameof(ServiceSettings));
                config.AddPlayTracing(environment);
            });

        services.AddHostedService<InventoryEventConsumingService>();
        services.AddScoped<ItemDemuliplexingHandler>();
        
        return services;
    }
}