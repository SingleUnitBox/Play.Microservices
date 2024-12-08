using Play.Catalog.Application.Exceptions;
using Play.Catalog.Domain.Repositories;
using Play.Common.Temp.Commands;

namespace Play.Catalog.Application.Commands.Handlers;

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