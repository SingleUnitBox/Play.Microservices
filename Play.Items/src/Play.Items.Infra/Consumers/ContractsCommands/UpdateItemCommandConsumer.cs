using MassTransit;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class UpdateItemCommandConsumer(
    IItemRepository itemRepository,
    IBusPublisher busPublisher,
    ICommandDispatcher commandDispatcher) : IConsumer<UpdateItem>
{
    
    public async Task Consume(ConsumeContext<UpdateItem> context)
    {
        var command = context.Message;
        var localCommand = new Application.Commands.UpdateItem(command.ItemId, command.Name, 
            command.Description, command.Price);
        
        await commandDispatcher.DispatchAsync(localCommand);
    }
    
    // public async Task Consume(ConsumeContext<UpdateItem> context)
    // {
    //     var command = context.Message;
    //     var item = await itemRepository.GetByIdAsync(command.ItemId);
    //     if (item is null)
    //     {
    //         throw new ItemNotFoundException(command.ItemId);
    //     }
    //
    //     item.Name = command.Name;
    //     item.Description = command.Description;
    //     item.Price = command.Price;
    //
    //     await itemRepository.UpdateAsync(item);
    //     await busPublisher.PublishAsync(new ItemUpdated(
    //         item.Id, item.Name, item.Price));
    // }
}