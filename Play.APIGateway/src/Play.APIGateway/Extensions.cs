using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using Play.Common.Context;
using Play.Common.Messaging;
using Play.Common.Messaging.CorrelationContext;

namespace Play.APIGateway;

public static class Extensions
{
    public static WebApplication PublishCommand<TCommand>(
        this WebApplication app,
        string route,
        HttpMethod method,
        params (string routeKey, string propertyName)[] routeMap)
        where TCommand : class, ICommand
    {
        async Task<IResult> Handler(
            HttpContext context,
            TCommand command,
            IBusPublisher busPublisher,
            IContext idContext)
        {
            foreach (var (routeKey, propertyName) in routeMap)
            {
                if (context.Request.RouteValues.TryGetValue(routeKey, out var value) &&
                    Guid.TryParse(value?.ToString(), out var guid))
                {
                    var prop = typeof(TCommand).GetProperty(propertyName);
                    prop?.SetValue(command, guid);
                }
            }
    
            var correlationId = Guid.NewGuid();
            var userId = idContext.IdentityContext?.UserId ?? Guid.Empty;
    
            await busPublisher.PublishAsync(
                message: command,
                exchangeName: typeof(TCommand).GetExchangeName(),
                routingKey: "",
                correlationContext: new CorrelationContext(correlationId, userId));
    
            context.Response.Headers["RequestId"] = correlationId.ToString();
            return Results.Accepted($"play-operations/{correlationId}");
        }
        
        app.MapMethods(route, new[] { method.Method }, (Delegate)Handler);
        return app;
    }
    //
    // public static WebApplication PublishDeleteCommand<TCommand>(this WebApplication app,
    //     string route)
    //     where TCommand : class
    // {
    //     app.MapDelete(route + "/{id?}",
    //         async (
    //             [FromRoute] Guid? id,
    //             [FromServices] IBusPublisher busPublisher) =>
    //         {
    //             TCommand command;
    //             var commandType = typeof(TCommand);
    //             var idProperty = commandType.GetProperties()
    //                 .FirstOrDefault(p => p.PropertyType == typeof(Guid));
    //
    //             var constructor = commandType.GetConstructors().FirstOrDefault();
    //
    //             var args = new List<object>();
    //             if (id.HasValue)
    //             {
    //                 args.Add(id.Value);
    //             }
    //             else
    //             {
    //                 args.Clear();
    //             }
    //             
    //             command = (TCommand)constructor.Invoke(args.ToArray());
    //             
    //             await busPublisher.PublishAsync(
    //                 message: command,
    //                 exchangeName: typeof(TCommand).GetExchangeName());
    //             return Results.Accepted();
    //         });
    //
    //     return app;
    // }

    public static IEndpointRouteBuilder MapCommands(this IEndpointRouteBuilder app,
        Action<CommandRouteBuilder> configure)
    {
        var builder = new CommandRouteBuilder(app);
        configure(builder);

        return app;
    }
}

public class CommandRouteBuilder(IEndpointRouteBuilder app)
{
    public CommandRouteBuilder Post<TCommand>(string route)
        where TCommand : class, ICommand
    {
        MapCommand<TCommand>(route, HttpMethod.Post);
        return this;
    }
    
    public CommandRouteBuilder Post<TCommand>(string route, params (string route, string property)[] mappings)
        where TCommand : class, ICommand
    {
        MapCommand<TCommand>(route, HttpMethod.Post, mappings);
        return this;
    }
    
    public CommandRouteBuilder Put<TCommand>(string route)
        where TCommand : class, ICommand
    {
        MapCommand<TCommand>(route, HttpMethod.Put);
        return this;
    }
    
    public CommandRouteBuilder Delete<TCommand>(string route) 
        where TCommand : class, ICommand
    {
        app.MapDelete(route + "/{id?}", async (
            [FromRoute] Guid? id,
            [FromServices] IBusPublisher publisher,
            [FromServices] IContext context) =>
        {
            var command = CreateCommandWithId<TCommand>(id);
            await PublishCommand(command, publisher, context);
            return Results.Accepted();
        });
        return this;
    }

    private void MapCommand<TCommand>(
        string route,
        HttpMethod method,
        params (string route, string propertyName)[] routeMap)
        where TCommand : class, ICommand
    {
        app.MapMethods(route, new[] { method.Method }, async (
            HttpContext httpContext,
            [FromBody] TCommand command,
            [FromServices] IBusPublisher busPublisher,
            [FromServices] IContext context) =>
        {
            foreach (var (routeKey, propertyName) in routeMap)
            {
                if (httpContext.Request.RouteValues.TryGetValue(routeKey, out var value)
                    && Guid.TryParse(value?.ToString(), out var guidId))
                {
                    typeof(TCommand).GetProperty(propertyName)?.SetValue(command, guidId);
                }
            }

            var correlationId = Guid.NewGuid();
            await PublishCommand(command, busPublisher, context);
            
            httpContext.Response.Headers["RequestId"] = correlationId.ToString();
            return Results.Accepted($"play-operations/{correlationId}");
        });
    }

    private async Task PublishCommand<TCommand>(
        TCommand command,
        IBusPublisher busPublisher,
        IContext context)
        where  TCommand : class, ICommand
    {
        var correlationId = Guid.NewGuid();
        var userId = context.IdentityContext?.UserId ?? Guid.Empty;

        await busPublisher.PublishAsync(
            command,
            typeof(TCommand).GetExchangeName(),
            routingKey: "",
            correlationContext: new CorrelationContext(correlationId, userId));
    }
    
    private static TCommand CreateCommandWithId<TCommand>(Guid? id) where TCommand : class
    {
        var constructor = typeof(TCommand).GetConstructors().First();
        var args = id.HasValue ? new object[] { id.Value } : Array.Empty<object>();
        return (TCommand)constructor.Invoke(args);
    }
}