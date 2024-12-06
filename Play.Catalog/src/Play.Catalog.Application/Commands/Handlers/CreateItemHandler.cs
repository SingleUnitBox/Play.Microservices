using Play.Catalog.Application.Exceptions;
using Play.Catalog.Domain.Entities;
using Play.Catalog.Domain.Repositories;
using Play.Common.Temp.Commands;

namespace Play.Catalog.Application.Commands.Handlers;

public class CreateItemHandler : ICommandHandler<CreateItem>
{
    private readonly IItemRepository _itemRepository;

    public CreateItemHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task HandleAsync(CreateItem command)
    {
        var item = await _itemRepository.GetByIdAsync(command.Id);
        if (item is not null)
        {
            throw new ItemAlreadyExistException(item.Id);
        }
        
        item = new Item(command.Id, command.Name, command.Description, command.Price);
        await _itemRepository.CreateAsync(item);
    }
}