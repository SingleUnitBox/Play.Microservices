﻿using MassTransit;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.User.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.User;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMoneyBagRepository _moneyBagRepository;
    
    public UserCreatedConsumer(IPlayerRepository playerRepository,
        IMoneyBagRepository moneyBagRepository)
    {
        _playerRepository = playerRepository;
        _moneyBagRepository = moneyBagRepository;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;

        var player = await _playerRepository.GetByIdAsync(message.UserId);
        if (player is not null)
        {
            return;
        }

        player = Player.Create(message.UserId, message.Username);
        await _playerRepository.CreateAsync(player);
        
        var moneyMag = MoneyBag.Create(player.Id, 100);
        await _moneyBagRepository.CreateMoneyBag(moneyMag);
    }
}