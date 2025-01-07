using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;

namespace Play.Common.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICommandHandler<>).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();
        services.Scan(a => a.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
    
    public static WebApplication MapCommandEndpoint<TCommand>(this WebApplication app,
        string route,
        HttpMethod method)
        where TCommand : class
    {
        app.MapMethods(route, new[] { method.Method }, 
            async ([FromBody] TCommand command, [FromServices] IBusPublisher busPublisher) =>
            {
                await busPublisher.PublishAsync(command, Guid.NewGuid());
                return Results.Accepted();
            });
        
        return app;
    }
    
    public static WebApplication MapCommandEndpointWithItemId<TCommand>(this WebApplication app,
        string route,
        HttpMethod method)
        where TCommand : class
    {
        app.MapMethods(route + "/{itemId}", new[] { method.Method }, 
            async (
                [FromBody] TCommand command,
                [FromServices] IBusPublisher busPublisher,
                [FromRoute] Guid itemId) =>
            {
                var itemIdProperty = typeof(TCommand).GetProperty("ItemId");
                if (itemIdProperty is not null && itemIdProperty.PropertyType == typeof(Guid))
                {
                    itemIdProperty.SetValue(command, itemId);
                }

                await busPublisher.PublishAsync(command, Guid.NewGuid());
                return Results.Accepted();
            });
        
        return app;
    }
}