using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class DeleteItemHandler(
    IItemRepository itemRepository,
    IBusPublisher busPublisher) : ICommandHandler<DeleteItem>
{
    public async Task HandleAsync(DeleteItem command)
    {
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }

        await itemRepository.DeleteAsync(item.Id);
        await busPublisher.Publish(new ItemDeleted(item.Id));
    }
}