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
        var item = new Item(command.Name, command.Description, command.Price);
        
        await _itemRepository.CreateAsync(item);
    }
}