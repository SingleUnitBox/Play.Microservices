using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Play.Common.Messaging.Consumers;
using Play.Items.Infra.Services.Demultiplexing;

namespace Play.Items.Infra.Services.Consumers;

public class NonGenericCommandConsumerService(ICommandConsumer commandConsumer,
    IServiceProvider serviceProvider) : BackgroundService
{
    public const string ItemChangesQueue = "item_changes_queue";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await commandConsumer.ConsumeNonGenericCommand(
            async messageData =>
            {
                var demultiplexingHandler = CreateDemultiplexingHandler();
                await demultiplexingHandler.HandleAsync(messageData);
            },
            ItemChangesQueue,
            stoppingToken);
    }

    private ItemChangesHandler CreateDemultiplexingHandler()
    {
        var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetService<ItemChangesHandler>();
    }
}