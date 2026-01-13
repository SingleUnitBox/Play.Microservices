using Play.Common.Abs.Events;
using Play.World.Application.Exceptions;
using Play.World.Domain.Entities;
using Play.World.Domain.Repositories;
using Play.World.Domain.ValueObjects;

namespace Play.World.Application.Events.User.Handlers;

public class UserCreatedHandler(IPlayerRepository playerRepository) : IEventHandler<UserCreated>
{
    public async Task HandleAsync(UserCreated @event)
    {
        var player = await playerRepository.GetByIdAsync(@event.UserId);
        if (player is not null)
        {
            throw new PlayerAlreadyExistException(@event.UserId);
        }
        
        var positon = Coordinate.CreateRandom();
        player = Player.Create(@event.UserId, @event.Username, positon);
        
        await playerRepository.AddAsync(player);
    }
}

