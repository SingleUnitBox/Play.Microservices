using Play.Common.Abs.Commands;

namespace Play.Common.RabbitMq.Subscribers;

public interface ICommandSubscriber
{
    Task SubscribeCommand<TCommand>() where TCommand : class, ICommand;
}