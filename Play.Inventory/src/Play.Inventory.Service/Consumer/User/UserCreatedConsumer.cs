using MassTransit;
using Play.Common;
using Play.User.Contracts;

namespace Play.Inventory.Service.Consumer.User;

public class UserCreatedConsumer : IConsumer<Contracts.UserCreated>
{
    private readonly IRepository<Entities.User> _userRepository;

    public UserCreatedConsumer(IRepository<Entities.User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<Contracts.UserCreated> context)
    {
        var user = await _userRepository.GetByIdAsync(context.Message.UserId);
        if (user is not null)
        {
            throw new InvalidOperationException($"User with id '{user.Id}' already exists.");
        }

        user = new Entities.User
        {
            Id = context.Message.UserId,
            Username = context.Message.Username,
        };
        
        await _userRepository.CreateAsync(user);
    }
}