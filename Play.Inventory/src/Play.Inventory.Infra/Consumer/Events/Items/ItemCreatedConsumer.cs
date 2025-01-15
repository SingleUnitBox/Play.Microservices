using MassTransit;
using Play.Common.Abs.Events;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Events.Items;

public class ItemCreatedConsumer : IConsumer<ItemCreated>
{
    private readonly IEventHandler<ItemCreated> _eventHandler;

    public ItemCreatedConsumer(IEventHandler<ItemCreated> eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task Consume(ConsumeContext<ItemCreated> context)
    {
        await _eventHandler.HandleAsync(context.Message);
    }
}