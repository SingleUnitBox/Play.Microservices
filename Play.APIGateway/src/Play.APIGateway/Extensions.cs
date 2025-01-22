using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;

namespace Play.APIGateway;

public static class Extensions
{
    public static WebApplication PublishCommand<TCommand>(this WebApplication app,
        string route,
        HttpMethod method)
        where TCommand : class, ICommand
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

                await busPublisher.Publish<TCommand>(command);
                return Results.Accepted();
            });

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
                
                await busPublisher.Publish(command);
                return Results.Accepted();
            });

        return app;
    }
}