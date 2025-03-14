using Play.Common.Abs.Commands;
using Play.Common.RabbitMq.Message;

namespace Play.Common.RabbitMq.Consumers;

public interface ICommandConsumer
{
    Task ConsumeCommand<TCommand>(CancellationToken stoppingToken) where TCommand : class, ICommand;
    Task ConsumeNonGenericCommand(
        Func<MessageData, Task> handleRawPayload,
        CancellationToken stoppingToken = default);
}