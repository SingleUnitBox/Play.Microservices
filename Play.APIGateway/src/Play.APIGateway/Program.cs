using Play.Common.Commands;
using Play.Common.Logging;
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

        builder.Host.UseSerilogWithSeq();
        var app = builder.Build();
        
        app.UseRouting();
        app.MapReverseProxy();

        // Play.Items
        // this is async, goes to RabbitMq
        app.MapCommandEndpointLocal<CreateItem>("play-items/items", HttpMethod.Post);
        app.MapCommandEndpointLocal<UpdateItem>("play-items/items", HttpMethod.Put);
        app.MapDeleteCommandEndpointLocal<DeleteItem>("play-items/items");
        app.MapDeleteCommandEndpointLocal<DeleteItems>("play-items/items/delete");

        // Play.Inventory
        //app.MapCommandEndpoint<PurchaseItem>()
        app.Run();
    }
}