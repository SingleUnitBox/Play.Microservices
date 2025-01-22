using Play.Common.Abs.Commands;

namespace Play.Common.RabbitMq.Consumers;

public interface ICommandConsumer
{
    Task ConsumeCommand<TCommand>() where TCommand : class, ICommand;
}