namespace Play.Items.Contracts.Events.RejectedEvents;

public class CreateItemRejected
{
    public Guid ItemId { get; }
    public string Name { get; }
    public string Reason { get; }
    public string Code { get; }

    public CreateItemRejected(Guid itemId, string name, string reason, string code)
        => (ItemId, Name, Reason, Code) = (itemId, name, reason, code);
}