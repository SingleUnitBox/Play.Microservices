using Play.Common.Abs.Events;
using Play.World.Application.Exceptions;
using Play.World.Domain.Entities;
using Play.World.Domain.Repositories;
using Play.World.Domain.ValueObjects;

namespace Play.World.Application.Events.Items.Handlers;

public class ItemCreatedHandler(IItemLocationsRepository itemLocationsRepository) : IEventHandler<ItemCreated>
{
    public async Task HandleAsync(ItemCreated @event)
    {
        var itemLocation = await itemLocationsRepository.GetByItemIdAsync(@event.ItemId);
        if (itemLocation is not null)
        {
            throw new ItemLocationAlreadyExistException(@event.ItemId);
        }
        
        var random = new Random();
        var position = Coordinate.Create(new Random().Next(-180, 180), random.Next(-90, 90));
        itemLocation = ItemLocation.Create(@event.ItemId, @event.Name, position);
        
        await itemLocationsRepository.AddAsync(itemLocation);
    }
}