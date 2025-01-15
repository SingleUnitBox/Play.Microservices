using Play.Common.Abs.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.Handlers;

public class PlayerCreatedHandler : IEventHandler<PlayerCreated>
{
    private readonly IPlayerRepository _playerRepository; 
    private readonly IMoneyBagRepository _moneyBagRepository;

    public PlayerCreatedHandler(IPlayerRepository playerRepository, IMoneyBagRepository moneyBagRepository)
    {
        _playerRepository = playerRepository;
        _moneyBagRepository = moneyBagRepository;
    }

    public async Task HandleAsync(PlayerCreated @event)
    {
        var player = await _playerRepository.GetByIdAsync(@event.PlayerId);
        if (player is null)
        {
            throw new PlayerNotFoundException(@event.PlayerId);
        }

        var moneyBag = await _moneyBagRepository.GetMoneyBagByPlayerId(@event.PlayerId);
        if (moneyBag is not null)
        {
            return;
        }
        
        moneyBag = MoneyBag.Create(player.Id);
        await _moneyBagRepository.CreateMoneyBag(moneyBag);
    }
}