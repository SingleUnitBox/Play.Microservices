using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class DeleteItemsHandler(
    IItemRepository itemRepository,
    IBusPublisher busPublisher) : ICommandHandler<Contracts.Commands.DeleteItems>
{
    public async Task HandleAsync(Contracts.Commands.DeleteItems command)
    {
        var items = await itemRepository.GetAllAsync();
        if (items is not null && items.Any())
        {
            foreach (var item in items)
            {
                await itemRepository.DeleteAsync(item.Id);
                await busPublisher.PublishAsync(new ItemDeleted(item.Id));
            }
        }
    }
}