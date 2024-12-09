using Play.Common.Abs.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

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