using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateItemHandler : ICommandHandler<CreateItem>
{
    private readonly IItemRepository _itemRepository;
    private readonly IBusPublisher _busPublisher;

    public CreateItemHandler(IItemRepository itemRepository,
        IBusPublisher busPublisher)
    {
        _itemRepository = itemRepository;
        _busPublisher = busPublisher;
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
        await _busPublisher.PublishAsync(new ItemCreated
        {
            ItemId = item.Id, Name = item.Name, Price = item.Price
        });
    }
}