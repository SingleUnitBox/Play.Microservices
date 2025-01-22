using Microsoft.AspNetCore.Mvc;
using Play.APIGateway.Commands;
using Play.Common.Logging;
using Play.Common.RabbitMq;

namespace Play.APIGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        builder.Services.AddRabbitMq();
        builder.Host.UseSerilogWithSeq();
        var app = builder.Build();
        
        app.UseRouting();
        app.MapReverseProxy();
        // app.MapGet("publishCreateItem", async ([FromServices] CommandPublisher publisher) =>
        // {
        //     await publisher.PublishCommand(new CreateItem("Sword", "Good damage", 100.2m));
        // });

        // Play.Items
        // this is async, goes to RabbitMq
        app.PublishCommand<CreateItem>("play-items/items", HttpMethod.Post);
        app.PublishCommand<UpdateItem>("play-items/items", HttpMethod.Put);
        app.PublishDeleteCommand<DeleteItem>("play-items/items");
        app.PublishDeleteCommand<DeleteItems>("play-items/items/delete");

        // Play.Inventory
        //app.MapCommandEndpoint<PurchaseItem>()
        app.Run();
    }
}