using Play.Common.Abs.Events;
using Play.Common.Messaging.Message;
using Play.Common.Serialization;
using Play.Inventory.Application.Events.External.Items;

namespace Play.Inventory.Infra.Events.External;

public class ItemDemuliplexingHandler(
    IEventHandler<ItemCreated> itemCreatedHandler,
    IEventHandler<ItemUpdated> itemUpdatedHandler,
    IEventHandler<ItemDeleted> itemDeletedHandler,
    ISerializer serializer)
{
    public async Task HandleAsync(MessageData messageData, CancellationToken cancellationToken)
    {
        var @event = Demultiplex(messageData);
        switch (@event)
        {
            case ItemCreated itemCreated:
                await itemCreatedHandler.HandleAsync(itemCreated);
                break;
            
            case ItemUpdated itemUpdated:
                await itemUpdatedHandler.HandleAsync(itemUpdated);
                break;
            
            case ItemDeleted itemDeleted:
                await itemDeletedHandler.HandleAsync(itemDeleted);
                break;
        };
    }

    private IEvent Demultiplex(MessageData messageData)
    {
        return messageData.Type switch
        {
            nameof(ItemCreated) => serializer.DeserializeBinary<ItemCreated>(messageData.Payload),
            nameof(ItemUpdated) => serializer.DeserializeBinary<ItemUpdated>(messageData.Payload),
            nameof(ItemDeleted) => serializer.DeserializeBinary<ItemDeleted>(messageData.Payload),
            _ => throw new NotImplementedException()
        };
    }
}