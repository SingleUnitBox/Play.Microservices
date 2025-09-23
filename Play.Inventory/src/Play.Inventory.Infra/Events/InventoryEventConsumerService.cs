using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Events;
using Play.Common.Messaging;
using Play.Common.Messaging.Consumers;
using Play.Inventory.Application.Events.External.Items;
using Play.Inventory.Application.Events.External.Users;

namespace Play.Inventory.Infra.Events;

public class InventoryEventConsumerService(IEventConsumer eventConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await eventConsumer.ConsumeEvent<ItemCreated>(typeof(ItemCreated).GetQueueName(), stoppingToken: stoppingToken);
    }
}