using MassTransit;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Contracts.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class DeleteItemsCommandConsumer(
    IItemRepository itemRepository,
    IBusPublisher busPublisher,
    ICommandDispatcher commandDispatcher) : IConsumer<DeleteItems>
{
    
    public async Task Consume(ConsumeContext<DeleteItems> context)
    {
        await commandDispatcher.DispatchAsync(new Application.Commands.DeleteItems());
    }
    // public async Task Consume(ConsumeContext<DeleteItems> context)
    // {
    //     var items = await itemRepository.GetAllAsync();
    //     if (items is not null && items.Any())
    //     {
    //         foreach (var item in items)
    //         {
    //             await itemRepository.DeleteAsync(item.Id);
    //             await busPublisher.PublishAsync(new ItemDeleted(item.Id));
    //         }
    //     }
    // }
}