using Play.Common.Abs.Commands;
using Play.Common.Messaging.Message;

namespace Play.Common.Messaging.Consumers;

public interface ICommandConsumer
{
    Task ConsumeCommand<TCommand>(CancellationToken stoppingToken) where TCommand : class, ICommand;
    Task ConsumeNonGenericCommand(
        Func<MessageData, Task> handleRawPayload,
        string queue,
        CancellationToken stoppingToken = default);
}