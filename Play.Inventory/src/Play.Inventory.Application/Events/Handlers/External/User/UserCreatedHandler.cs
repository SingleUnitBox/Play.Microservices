using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.User.Contracts.Events;

namespace Play.Inventory.Application.Events.Handlers.External.User;

public class UserCreatedHandler : IEventHandler<UserCreated>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IBusPublisher _busPublisher;
    private readonly IEventDispatcher _eventDispatcher;

    public UserCreatedHandler(IPlayerRepository playerRepository,
        IBusPublisher busPublisher,
        IEventDispatcher eventDispatcher)
    {
        _playerRepository = playerRepository;
        _busPublisher = busPublisher;
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