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
    
    public static WebApplication PublishDeleteCommand<TCommand>(this WebApplication app,
        string route)
        where TCommand : class
    {
        app.MapDelete(route + "/{id?}",
            async (
                [FromRoute] Guid? id,
                [FromServices] IBusPublisher busPublisher) =>
            {
                TCommand command;
                var commandType = typeof(TCommand);
                var idProperty = commandType.GetProperties()
                    .FirstOrDefault(p => p.PropertyType == typeof(Guid));

                var constructor = commandType.GetConstructors().FirstOrDefault();

                var args = new List<object>();
                if (id.HasValue)
                {
                    args.Add(id.Value);
                }
                else
                {
                    args.Clear();
                }
                
                command = (TCommand)constructor.Invoke(args.ToArray());
                
                await busPublisher.PublishAsync(
                    message: command,
                    exchangeName: typeof(TCommand).GetExchangeName());
                return Results.Accepted();
            });

        return app;
    }
}