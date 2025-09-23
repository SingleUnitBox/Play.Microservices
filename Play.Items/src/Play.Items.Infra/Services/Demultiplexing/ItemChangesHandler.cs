using Play.Common.Abs.Commands;
using Play.Common.Messaging.Message;
using Play.Common.Serialization;
using Play.Items.Application.Commands;

namespace Play.Items.Infra.Services.Demultiplexing;

public class ItemChangesHandler(ISerializer serializer,
    ICommandHandler<CreateItem> createItemHandler,
    ICommandHandler<UpdateItem> updateItemHandler,
    ICommandHandler<DeleteItem> deleteItemHandler)
{
    public async Task HandleAsync(MessageData messageData, CancellationToken cancellationToken = default)
    {
        var command = Demultiplex(messageData);
        switch (command)
        {
            case CreateItem createItem:
                await createItemHandler.HandleAsync(createItem);
                break;
            case UpdateItem updateItem:
                await updateItemHandler.HandleAsync(updateItem);
                break;
            case DeleteItem deleteItem:
                await deleteItemHandler.HandleAsync(deleteItem);
                break;
            default:
                break;
        };
    }

    private ICommand Demultiplex(MessageData messageData)
    {
        switch (messageData.Type)
        {
            case "CreateItem":
                return serializer.DeserializeBinary<CreateItem>(messageData.Payload);
            case "UpdateItem":
                return serializer.DeserializeBinary<UpdateItem>(messageData.Payload);
            case "DeleteItem":
                return serializer.DeserializeBinary<DeleteItem>(messageData.Payload);
            default:
                return null;
        }
    }
}