using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Events;
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
        await busPublisher.PublishAsync(new ItemDeleted(item.Id));
    }
}