using Play.APIGateway.Commands.Inventory;
using Play.APIGateway.Commands.Items;
using Play.Common;
using Play.Common.Auth;
using Play.Common.Context;
using Play.Common.Logging;
using Play.Common.Messaging;
using Play.Common.Observability;
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
        builder.Services.AddRabbitMq(builder.Configuration, rabbitBuilder =>
            rabbitBuilder
                .AddConnectionProvider()
                .AddChannelFactory()
                .AddBusPublisher()
                .AddResiliency());
                //.AddTopologyInitializer());
        builder.Services.AddSerialization();
        builder.Host.UseSerilogWithSeq();
        builder.Services.AddContext();
        builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
        builder.Services.AddPlayMicroservice(builder.Configuration,
            config =>
            {
                config.AddPlayTracing(builder.Environment);
            });
        
        var app = builder.Build();
        app.UseRouting();
        app.MapReverseProxy();
        
        // Play.Items
        // this is async, goes to RabbitMq
        app.PublishCommand<CreateItem>("play-items/items", HttpMethod.Post);
        app.PublishCommand<MakeSocket>("play-items/items/{itemId}/socket", HttpMethod.Post, ("itemId", "ItemId"));
        app.PublishCommand<EmbedArtifact>("play-items/items/{itemId}/artifact", HttpMethod.Post, ("itemId", "ItemId"));
        app.PublishCommand<UpdateItem>("play-items/items", HttpMethod.Put);
        app.PublishCommand<CreateCrafter>("play-items/crafters", HttpMethod.Post);
        app.PublishDeleteCommand<DeleteItem>("play-items/items");
        app.PublishDeleteCommand<DeleteItems>("play-items/items/delete");

        // Play.Inventory
        app.PublishCommand<PurchaseItem>("play-inventory/items", HttpMethod.Post);
        
        app.Run();
    }
}