using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.MassTransit;
public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration,
        Assembly[] assemblies)
    {
        // services.AddMassTransit(configure =>
        // {
        //     //configure.AddConsumers(assemblies);
        //     configure.UsingRabbitMq((ctx, cfg) =>
        //     {
        //         var rabbitMqSettings = configuration
        //             .GetSection(nameof(RabbitMqSettings))
        //             .Get<RabbitMqSettings>();
        //         var serviceSettings = configuration
        //             .GetSection(nameof(ServiceSettings))
        //             .Get<ServiceSettings>();
        //         
        //         cfg.Host(rabbitMqSettings.Host);
        //         
        //         // var eventTypes = assemblies.SelectMany(a => a.GetTypes())
        //         //     .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        //         // foreach (var eventType in eventTypes)
        //         // {
        //         //     cfg.Message<IEvent>(x =>
        //         //         x.SetEntityName(eventType.Name));
        //         // }
        //
        //         cfg.ConfigureEndpoints(ctx, 
        //             new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
        //         
        //     });
        //
        //     services.AddMassTransitHostedService();
        // });
        
        return services;
    }
}