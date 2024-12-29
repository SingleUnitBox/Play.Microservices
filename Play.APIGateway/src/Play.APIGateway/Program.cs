using Play.Common.Commands;
using Play.Common.MassTransit;
using Play.Common.Messaging;
using Play.Items.Contracts.Commands;

namespace Play.APIGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        builder.Services.AddMessaging();
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());
        
        var app = builder.Build();
        app.MapReverseProxy();
        // Play.Items
        app.MapCommandEndpoint<CreateItem>("play-items/items", HttpMethod.Post);
        app.MapCommandEndpoint<UpdateItem>("play-items/items", HttpMethod.Put);
        app.MapCommandEndpoint<DeleteItem>("play-items/items", HttpMethod.Delete);
        // Play.Inventory
        app.Run();
    }
}