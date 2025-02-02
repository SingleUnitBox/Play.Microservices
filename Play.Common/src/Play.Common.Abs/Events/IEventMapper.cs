using Play.Common.Abs.SharedKernel.DomainEvents;

namespace Play.Common.Abs.Events;

public interface IEventMapper
{
    IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> domainEvents);
    IEvent Map(IDomainEvent domainEvent);
}