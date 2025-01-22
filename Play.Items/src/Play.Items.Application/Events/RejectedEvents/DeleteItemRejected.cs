namespace Play.Items.Application.Events.RejectedEvents;

public class DeleteItemRejected
{
    public Guid ItemId { get; }
    public string Reason { get; }
    public string Code { get; }

    public DeleteItemRejected(Guid itemId, string reason, string code)
        => (ItemId, Reason, Code) = (itemId, reason, code);
}