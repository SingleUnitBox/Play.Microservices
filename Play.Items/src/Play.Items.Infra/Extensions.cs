using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Items.Application.Services;
using Play.Items.Infra.Services;

namespace Play.Items.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        // generic consumer
        services.AddHostedService<CreateItemConsumerService>();
        
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