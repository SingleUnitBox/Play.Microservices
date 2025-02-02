using Play.Common.Abs.Events;

namespace Play.Items.Application.Events.RejectedEvents;

public record CreateItemRejected : IRejectedEvent
{
    public Guid ItemId { get; }
    public string Name { get; }
    public string Reason { get; }
    public string Code { get; }

    public CreateItemRejected(Guid itemId, string name,string reason, string code)
        => (ItemId, Name, Reason, Code) = (itemId, name, reason, code);
}