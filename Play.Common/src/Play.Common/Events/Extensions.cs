using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;
using Play.Common.Logging.Attributes;

namespace Play.Common.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(a => a.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                .WithoutAttribute<LoggingDecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}