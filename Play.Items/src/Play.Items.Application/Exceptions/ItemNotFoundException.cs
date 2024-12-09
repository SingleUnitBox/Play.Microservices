using Play.Common.Abs.Exceptions;

namespace Play.Items.Application.Exceptions;

public class ItemNotFoundException : PlayException
{
    public Guid ItemId { get; }
    public ItemNotFoundException(Guid itemId)
        : base($"Item with id '{itemId}' was not found.")
    {
        ItemId = itemId;
    }
}