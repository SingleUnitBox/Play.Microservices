using Play.APIGateway.Commands.Inventory;
using Play.APIGateway.Commands.Items;
using Play.Common.Auth;
using Play.Common.Context;
using Play.Common.Logging;
using Play.Common.RabbitMq;
using Play.Common.Serialization;
using Play.Common.Settings;

namespace Play.APIGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        var settings = builder.Services.GetSettings<ServiceSettings>(nameof(ServiceSettings));
        builder.Services.AddSingleton(settings);
        builder.Services.AddRabbitMq(rabbitBuilder =>
            rabbitBuilder
                .AddConnectionProvider()
                .AddChannelFactory());
        builder.Services.AddSerialization();
        builder.Host.UseSerilogWithSeq();
        builder.Services.AddContext();
        builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
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
        app.PublishCommand<PurchaseItem>("play-inventory/items", HttpMethod.Post);
        app.Run();
    }
}