using Play.Common.Abs.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class DeleteItemHandler : ICommandHandler<DeleteItem>
{
    private readonly IItemRepository _itemRepository;

    public DeleteItemHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task HandleAsync(DeleteItem command)
    {
        var item = await _itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }
        
        await _itemRepository.DeleteAsync(item.Id);
    }
}