using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Common.Messaging.Ordering;

public class IgnoreOutOfOrderEventDecorator<TEvent>(
    IEventHandler<TEvent> innerHandler,
    OutOfOrderDetector outOfOrderDetector,
    ILogger<IgnoreOutOfOrderEventDecorator<TEvent>> logger)
    : IEventHandler<TEvent> where TEvent : class, IEvent
{
    public async Task HandleAsync(TEvent @event)
    {
        if (typeof(IVersionedMessage).IsAssignableFrom(typeof(TEvent)) is false)
        {
            await innerHandler.HandleAsync(@event);
            return;
        }

        var isOutOfOrder = await outOfOrderDetector.Check(@event);
        if (isOutOfOrder)
        {
            logger.LogWarning($"[{DateTime.UtcNow:0}] Detected out of order event - skipping... Event: '{Environment.NewLine}' {@event}");
            return;
        }
        
        await innerHandler.HandleAsync(@event);
    }
}