using Play.Common.Abs.Events;

namespace Play.Items.Application.Events.RejectedEvents;

public record UpdateItemRejected : IRejectedEvent
{
    public Guid ItemId { get; }
    public string Reason { get; }
    public string Code { get; }
    public UpdateItemRejected(Guid itemId, string reason, string code)
        => (ItemId, Reason, Code) = (itemId, reason, code);
}