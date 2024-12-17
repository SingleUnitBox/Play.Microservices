using MassTransit;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class DeleteItemCommandConsumer(IItemRepository itemRepository,
    IBusPublisher busPublisher) : IConsumer<DeleteItem>
{
    public async Task Consume(ConsumeContext<DeleteItem> context)
    {
        var command = context.Message;
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }
        
        await itemRepository.DeleteAsync(item.Id);
        await busPublisher.PublishAsync(new ItemDeleted(item.Id));
    }
}