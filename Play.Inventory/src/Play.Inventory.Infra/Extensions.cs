using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common;
using Play.Common.Exceptions;
using Play.Common.Http;
using Play.Common.Messaging;
using Play.Common.Observability;
using Play.Common.Serialization;
using Play.Common.Settings;
using Play.Inventory.Application.Services.Clients;
using Play.Inventory.Infra.Events;
using Play.Inventory.Infra.Exceptions;
using Play.Inventory.Infra.Services.Clients;

namespace Play.Inventory.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddRabbitMq(builder =>
        {
            builder
                //.AddCommandConsumer()
                .AddEventConsumer()
                .AddConnectionProvider()
                .AddChannelFactory()
                .AddTopologyInitializer();
        });
        var httpClientSettings = services.GetSettings<HttpClientSettings>(nameof(HttpClientSettings));
        services.AddSingleton(httpClientSettings);
        services.AddHttpClient();
        services.AddSerialization();
        services.AddTransient<IUserServiceClient, UserServiceClient>();
        services.AddCommonHttpClient();

        services.AddPlayMicroservice(configuration,
            config =>
            {
                config.AddExceptionHandling();
                config.AddCustomExceptionToMessageMapper<InventoryExceptionToMessageMapper>();
                config.AddSettings<ServiceSettings>(nameof(ServiceSettings));
                config.AddPlayTracing(environment);
            });

        // services.AddHostedService<InventoryEventConsumerService>();
        // services.AddSingleton<IEventConsumer, InventoryEventConsumer>();
        
        return services;
    }
}