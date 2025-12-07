using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Items.Application.Events;
using Play.Items.Domain.DomainEvents;
using ItemCreated = Play.Items.Domain.DomainEvents.ItemCreated;
using ItemDeleted = Play.Items.Domain.DomainEvents.ItemDeleted;

namespace Play.Items.Infra.Services;

internal sealed class EventMapper : IEventMapper
{
    public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> domainEvents)
        => domainEvents.Select(Map).Distinct();

    public IEvent? Map(IDomainEvent domainEvent)
        => domainEvent switch
        {
            ItemCreated dEvent => new Application.Events.ItemCreated(dEvent.ItemId, dEvent.Name, dEvent.Price),
            //NameUpdated dEvent => new ItemUpdated(dEvent.ItemId, dEvent.Name, dEvent.Price),
            //DescriptionUpdated dEvent => new ItemUpdated(dEvent.ItemId, dEvent.Name, dEvent.Price),
            PriceUpdated dEvent => new ItemUpdated(dEvent.ItemId, dEvent.Name, dEvent.Price),
            ItemDeleted dEvent => new Application.Events.ItemDeleted(dEvent.ItemId),
            _ => null
        };
}