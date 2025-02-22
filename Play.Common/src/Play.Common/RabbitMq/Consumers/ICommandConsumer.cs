using Play.Common.Abs.Commands;

namespace Play.Common.RabbitMq.Consumers;

public interface ICommandConsumer
{
    Task ConsumeCommand<TCommand>(CancellationToken stoppingToken) where TCommand : class, ICommand;
}