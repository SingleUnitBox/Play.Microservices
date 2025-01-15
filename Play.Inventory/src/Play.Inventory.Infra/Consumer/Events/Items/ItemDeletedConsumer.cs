using MassTransit;
using Play.Common.Abs.Events;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Events.Items;

public class ItemDeletedConsumer : IConsumer<ItemDeleted>
{
    private readonly IEventHandler<ItemDeleted> _eventHandler;

    public ItemDeletedConsumer(IEventHandler<ItemDeleted> eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task Consume(ConsumeContext<ItemDeleted> context)
    {
        await _eventHandler.HandleAsync(context.Message);
    }
}