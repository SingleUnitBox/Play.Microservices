using Play.Common.Abs.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class UpdateItemHandler : ICommandHandler<UpdateItem>
{
    private readonly IItemRepository _itemRepository;

    public UpdateItemHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task HandleAsync(UpdateItem command)
    {
        var item = await _itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }
        
        item.Name = command.Name;
        item.Description = command.Description;
        item.Price = command.Price;
        
        await _itemRepository.UpdateAsync(item);
    }
}