using Play.Common.Abs.Commands;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateItemWithCrafterHandler : ICommandHandler<CreateItemWithCrafter>
{
    private readonly IItemRepository _itemRepository;

    public CreateItemWithCrafterHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public async Task HandleAsync(CreateItemWithCrafter command)
    {
        var crafter = Crafter.Create(command.CrafterName);
        var item = Item.Create(command.Name, command.Description, command.Price, DateTimeOffset.UtcNow);
        item.SetCrafter(crafter);
        
        await _itemRepository.CreateAsync(item);
    }
}