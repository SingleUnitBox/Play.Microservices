using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using Play.Common.Logging.Attributes;
using Play.Common.Messaging.Executor;
using Play.Common.PostgresDb.UnitOfWork.Decorators;

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
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))
                .WithoutAttribute<LoggingDecoratorAttribute>()
                .WithoutAttribute<MessageExecutorDecoratorAttribute>()
                .WithoutAttribute<UnitOfWorkDecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
    
    public static WebApplication MapCommandEndpoint<TCommand>(this WebApplication app,
        string route,
        HttpMethod method)
        where TCommand : class
    {
        app.MapMethods(route + "/{id?}", new[] { method.Method }, 
            async (
                [FromBody] TCommand command,
                [FromServices] IBusPublisher busPublisher,
                [FromRoute] Guid? id) =>
            {
                if (id.HasValue)
                {
                    var idProperty = typeof(TCommand).GetProperties()
                        .FirstOrDefault(p => p.PropertyType == typeof(Guid));
                    if (idProperty is not null)
                    {
                        idProperty.SetValue(command, id);
                    }
                }
                
                await busPublisher.Publish(command);
                return Results.Accepted();
            });
        
        return app;
    }
}