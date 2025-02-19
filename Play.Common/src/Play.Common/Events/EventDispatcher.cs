using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Events;

namespace Play.Common.Events;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task HandleAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IEventHandler<TEvent>>();
        
        await handler?.HandleAsync(@event);
    }
}