using MassTransit;
using Play.Common;
using Play.User.Contracts;

namespace Play.Inventory.Service.Consumer.User;

public class UsernameUpdatedConsumer : IConsumer<Contracts.UsernameUpdated>
{
    private readonly IRepository<Entities.User> _userRepository;

    public UsernameUpdatedConsumer(IRepository<Entities.User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<Contracts.UsernameUpdated> context)
    {
        var user = await _userRepository.GetByIdAsync(context.Message.UserId);
        if (user is null)
        {
            return;
        }

        user.Username = context.Message.Username;
        await _userRepository.UpdateAsync(user);
    }
}