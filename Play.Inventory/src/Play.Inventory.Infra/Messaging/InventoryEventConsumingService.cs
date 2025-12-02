using Microsoft.Extensions.Hosting;
using Play.Common.Messaging.Consumers;

namespace Play.Inventory.Infra.Messaging;

public class InventoryEventConsumingService(
    IEventConsumer eventConsumer) : BackgroundService
{
    public const string ItemChangesQueue = "item_changes_queue";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await eventConsumer.ConsumeNonGenericEvent(
            async (messageData) =>
            {
                var demultiplexedHandler = CreateDemultiplexedHandler();
            },
            queue: ItemChangesQueue,
            cancellationToken: stoppingToken);
    }
    
    private 
}