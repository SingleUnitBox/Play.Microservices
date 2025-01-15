using MassTransit;
using Play.Common.Abs.Events;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Events.Items;

public class ItemUpdatedConsumer : IConsumer<ItemUpdated>
{
    private readonly IEventHandler<ItemUpdated> _eventHandler;

    public ItemUpdatedConsumer(IEventHandler<ItemUpdated> eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task Consume(ConsumeContext<ItemUpdated> context)
    {
        await _eventHandler.HandleAsync(context.Message);
    }
}