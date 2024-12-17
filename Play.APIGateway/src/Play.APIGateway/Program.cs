using MassTransit;
using Play.APIGateway.Settings;
using Play.Items.Contracts;
using Play.Items.Contracts.Commands;

namespace Play.APIGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitMqSetttings = builder.Configuration
                    .GetSection(nameof(RabbitMqSettings))
                    .Get<RabbitMqSettings>();
                var serviceSettings = builder.Configuration
                    .GetSection(nameof(ServiceSettings))
                    .Get<ServiceSettings>();
                
                cfg.Host(rabbitMqSetttings.Host);
                cfg.ConfigureEndpoints(ctx, 
                    new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
            });
        });
        builder.Services.AddMassTransitHostedService();
        var app = builder.Build();
        app.MapReverseProxy();
        app.MapPost("play-items/items", async (CreateItem command, IPublishEndpoint endpoint) =>
        {
            await endpoint.Publish(command, context =>
            {
                context.CorrelationId = Guid.NewGuid();
            });
            return Results.Accepted();
        });
        // app.MapPut("play-items/items", async (UpdateItem command, IPublishEndpoint endpoint) =>
        // {
        //     await endpoint.Publish(command);
        //     return Results.Accepted();
        // });
        // app.MapDelete("play-items/items", async (DeleteItem command, IPublishEndpoint endpoint) =>
        // {
        //     await endpoint.Publish(command);
        //     return Results.Accepted();
        // });
        app.Run();
    }
}