using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;

namespace Play.Common.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(a => a.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}