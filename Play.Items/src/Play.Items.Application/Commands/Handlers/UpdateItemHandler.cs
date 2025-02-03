using Play.Common.Abs.Commands;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Services;
using Play.Items.Domain.Repositories;
using IBusPublisher = Play.Common.Abs.RabbitMq.IBusPublisher;

namespace Play.Items.Application.Commands.Handlers;

public class UpdateItemHandler(
    IItemRepository itemRepository,
    IEventProcessor eventProcessor) : ICommandHandler<UpdateItem>
{
    public async Task HandleAsync(UpdateItem command)
    {
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }

        item.UpdateName(command.Name);
        item.UpdateDescription(command.Description);
        item.UpdatePrice(command.Price);

        await itemRepository.UpdateAsync(item);
        await eventProcessor.Process(item.Events);
    }
}