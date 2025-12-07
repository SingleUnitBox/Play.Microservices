using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Services;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class DeleteItemHandler(
    IItemRepository itemRepository,
    IEventProcessor eventProcessor) : ICommandHandler<DeleteItem>
{
    public async Task HandleAsync(DeleteItem command)
    {
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }

        item.Delete();
        await itemRepository.DeleteAsync(item.Id);
        await eventProcessor.Process(item.Events);
    }
}