using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Messaging;

namespace Play.APIGateway;

public static class Extensions
{
    public static WebApplication MapCommandEndpointWithItemId<TCommand>(this WebApplication app,
        string route,
        HttpMethod method)
        where TCommand : class
    {
        app.MapMethods(route + "/{id}", new[] { method.Method }, 
            async (
                [FromBody] TCommand command,
                [FromServices] IBusPublisher busPublisher,
                [FromRoute] Guid id) =>
            {
                var itemIdProperty = typeof(TCommand).GetProperties().FirstOrDefault(p => p.GetType() == typeof(Guid));
                if (itemIdProperty is not null)
                {
                    itemIdProperty.SetValue(command, id);
                }

                await busPublisher.PublishAsync(command, Guid.NewGuid());
                return Results.Accepted();
            });
        
        return app;
    }
}