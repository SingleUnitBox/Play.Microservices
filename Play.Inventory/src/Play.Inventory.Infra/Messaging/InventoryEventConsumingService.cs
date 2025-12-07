using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Play.Common.Messaging.Consumers;
using Play.Inventory.Infra.Events.External;

namespace Play.Inventory.Infra.Messaging;

public class InventoryEventConsumingService(
    IEventConsumer eventConsumer,
    IServiceProvider serviceProvider) : BackgroundService
{
    public const string ItemChangesQueue = "item_changes_queue";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await eventConsumer.ConsumeNonGenericEvent(
            async (messageData) =>
            {
                var demultiplexedHandler = CreateDemultiplexingHandler();
                await demultiplexedHandler.HandleAsync(messageData, stoppingToken);
            },
            queue: ItemChangesQueue,
            cancellationToken: stoppingToken);
    }

    private ItemDemuliplexingHandler CreateDemultiplexingHandler()
    {
        var iocScope = serviceProvider.CreateScope();
        return iocScope.ServiceProvider.GetRequiredService<ItemDemuliplexingHandler>();
    }
}