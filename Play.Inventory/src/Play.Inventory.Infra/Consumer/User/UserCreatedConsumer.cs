using MassTransit;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.User.Contracts;

namespace Play.Inventory.Infra.Consumer.User;

public class UserCreatedConsumer : IConsumer<Contracts.UserCreated>
{
    private readonly IUserRepository _userRepository;
    private readonly IMoneyBagRepository _moneyBagRepository;
    
    public UserCreatedConsumer(IUserRepository userRepository,
        IMoneyBagRepository moneyBagRepository)
    {
        _userRepository = userRepository;
        _moneyBagRepository = moneyBagRepository;
    }

    public async Task Consume(ConsumeContext<Contracts.UserCreated> context)
    {
        var message = context.Message;

        var user = await _userRepository.GetByIdAsync(message.UserId);
        if (user is not null)
        {
            return;
        }

        user = Domain.Entities.User.Create(message.UserId, message.Username);
        await _userRepository.CreateAsync(user);
        
        var moneyMag = MoneyBag.Create(user.Id, 100);
        await _moneyBagRepository.CreateMoneyBag(moneyMag);
    }
}