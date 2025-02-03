namespace Play.Common.Abs.SharedKernel.DomainEvents;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : class, IDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent);
}