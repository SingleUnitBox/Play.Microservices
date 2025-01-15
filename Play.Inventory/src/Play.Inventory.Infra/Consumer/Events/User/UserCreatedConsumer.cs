using MassTransit;
using Play.Common.Abs.Events;
using Play.User.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Events.User;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IEventHandler<UserCreated> _userCreatedHandler;

    public UserCreatedConsumer(IEventHandler<UserCreated> userCreatedHandler)
    {
        _userCreatedHandler = userCreatedHandler;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        await _userCreatedHandler.HandleAsync(context.Message);
    }
}