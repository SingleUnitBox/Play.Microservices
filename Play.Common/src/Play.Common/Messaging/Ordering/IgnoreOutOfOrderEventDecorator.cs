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

        var outOfOrder = outOfOrderDetector.Check(@event);
        if ()
    }
}