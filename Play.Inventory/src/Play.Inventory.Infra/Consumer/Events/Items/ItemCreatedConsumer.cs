using MassTransit;
using Play.Common.Abs.Events;
using Play.Inventory.Application.Events.External.Items;

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