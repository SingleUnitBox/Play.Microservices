using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Abs.SharedKernel.Types;

namespace Play.Common.Abs.SharedKernel;

public abstract class AggregateRoot<TKey>
{
    public TKey Id { get; protected set; }
    public int Version { get; protected set; }
    private List<IDomainEvent> _events = new();
    public IEnumerable<IDomainEvent> Events => _events;
    private bool _isVersionIncremented;

    protected void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any() && !_isVersionIncremented)
        {
            Version++;
        }

        _events.Add(@event);
        _isVersionIncremented = true;
    }

    public void ClearEvents() => _events.Clear();

    protected void IncrementVersion()
    {
        if (_isVersionIncremented)
        {
            return;
        }

        Version++;
        _isVersionIncremented = true;
    }

    public IEnumerable<IDomainEvent> GetEvents() => _events.ToList();
}

public class AggregateRoot : AggregateRoot<AggregateRootId>
{

}