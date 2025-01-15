namespace Play.Common.Abs.Events;

public interface IEventDispatcher
{
    Task HandleAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
}