using Play.Common.Abs.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.External.Users.Handlers;

public class UserCreatedHandler : IEventHandler<UserCreated>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IEventDispatcher _eventDispatcher;


    public UserCreatedHandler(IPlayerRepository playerRepository,
        IEventDispatcher eventDispatcher)
    {
        _playerRepository = playerRepository;
        _eventDispatcher = eventDispatcher;
    }
    
    public async Task HandleAsync(UserCreated @event)
    {
        var player = await _playerRepository.GetByIdAsync(@event.UserId);
        if (player is not null)
        {
            return;
        }

        player = Player.Create(@event.UserId, @event.Username);
        await _playerRepository.CreateAsync(player);
        
        await _eventDispatcher.HandleAsync(new PlayerCreated(@event.UserId));
    }
}