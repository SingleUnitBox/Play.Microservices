using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Items.Application.Events;
using Play.Items.Domain.DomainEvents;
using ArtifactAdded = Play.Items.Domain.DomainEvents.ArtifactAdded;
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
            ItemCreated dEvent => new Application.Events.ItemCreated(dEvent.ItemId, dEvent.Name, dEvent.Price, 1),
            SocketCreated dEvent => new ItemUpdated(dEvent.Item.Id, dEvent.Item.Name, dEvent.Item.Price, dEvent.Item.Version),
            ArtifactAdded dEvent => new Application.Events.ArtifactAdded(dEvent.Item.Id, dEvent.Item.Socket., dEvent.Item.Version),
            NameUpdated dEvent => new ItemUpdated(dEvent.Item.Id, dEvent.Item.Name, dEvent.Item.Price, dEvent.Item.Version),
            DescriptionUpdated dEvent => new ItemUpdated(dEvent.Item.Id, dEvent.Item.Name, dEvent.Item.Price, dEvent.Item.Version),
            PriceUpdated dEvent => new ItemUpdated(dEvent.Item.Id, dEvent.Item.Name, dEvent.Item.Price, dEvent.Item.Version),
            ItemDeleted dEvent => new Application.Events.ItemDeleted(dEvent.ItemId),
            _ => null
        };
}