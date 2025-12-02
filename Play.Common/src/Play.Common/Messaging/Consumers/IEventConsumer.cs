using Play.Common.Abs.Events;
using Play.Common.Messaging.Message;

namespace Play.Common.Messaging.Consumers;

public interface IEventConsumer
{
    Task ConsumeEvent<TEvent>(string? queueName = null, CancellationToken stoppingToken = default)
        where TEvent : class, IEvent;
    
    Task ConsumeNonGenericEvent(
        Func<MessageData, Task> handleRawPayload,
        string queue,
        CancellationToken cancellationToken = default);
}