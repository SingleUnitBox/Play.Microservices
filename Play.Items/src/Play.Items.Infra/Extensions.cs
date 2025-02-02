using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;
using Play.Items.Application.Services;
using Play.Items.Infra.Services;

namespace Play.Items.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddScoped<IMessageBroker, MessageBroker>();
        services.AddSingleton<IEventMapper, EventMapper>();
        
        return services;
    }
}