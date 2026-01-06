using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;
using Play.Common.Logging.Attributes;
using Play.Common.Messaging.Ordering;
using Play.Common.Messaging.Ordering.Attributes;

namespace Play.Common.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .OrderBy(a => a.GetName().Name)
            .ToList();
        var baseDir = AppContext.BaseDirectory;
        foreach (var dll in Directory.EnumerateFiles(baseDir, "Play.*.dll", SearchOption.TopDirectoryOnly))
        {
            var name = AssemblyName.GetAssemblyName(dll);
            if (assemblies.Any(a => AssemblyName.ReferenceMatchesDefinition(a.GetName(), name)))
                continue;

            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
            assemblies.Add(asm);
        }

        services.Scan(a => a.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                .WithoutAttribute<LoggingDecoratorAttribute>()
                .WithoutAttribute<OutOfOrderEventDecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}