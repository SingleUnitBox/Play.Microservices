using Play.Common.Abs.SharedKernel.DomainEvents;

namespace Play.Items.Domain.DomainEvents.Handlers;

public class NameUpdatedHandler : IDomainEventHandler<NameUpdated>
{
    public Task HandleAsync(NameUpdated domainEvent)
    {
        return Task.CompletedTask;
    }
}