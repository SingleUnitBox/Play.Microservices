using Play.Common.Abs.SharedKernel.DomainEvents;

namespace Play.Items.Application.Services;

public interface IEventProcessor
{
    Task Process(IEnumerable<IDomainEvent> domainEvents);
}