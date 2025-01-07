using MassTransit;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers;

public class UpdateItemConsumer(IItemRepository itemRepository, IBusPublisher busPublisher) : IConsumer<UpdateItem>
{
    public async Task Consume(ConsumeContext<UpdateItem> context)
    {
        var command = context.Message;
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }

        item.Name = command.Name;
        item.Description = command.Description;
        item.Price = command.Price;

        await itemRepository.UpdateAsync(item);
        await busPublisher.PublishAsync(new ItemUpdated(
            item.Id, item.Name, item.Price));
    }
}