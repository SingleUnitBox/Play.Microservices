using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Serialization;
using Play.Items.Application.Services;
using Play.Items.Infra.Services;
using Play.Items.Infra.Services.Consumers;
using Play.Items.Infra.Services.Demultiplexing;

namespace Play.Items.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        // generic consumer
        services.AddHostedService<CreateItemConsumerService>();
        services.AddHostedService<NonGenericCommandConsumerService>();
        services.AddScoped<ItemChangesHandler>();

        services.AddSerialization();
        services.AddScoped<IMessageBroker, MessageBroker>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddSingleton<IEventMapper, EventMapper>();
        services.Scan(a => a.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}