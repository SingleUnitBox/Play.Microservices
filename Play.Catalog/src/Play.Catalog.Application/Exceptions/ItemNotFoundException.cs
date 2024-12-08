using Play.Common.Temp.Exceptions;

namespace Play.Catalog.Application.Exceptions;

public class ItemNotFoundException : PlayException
{
    public Guid ItemId { get; }
    public ItemNotFoundException(Guid itemId)
        : base($"Item with id '{itemId}' was not found.")
    {
        ItemId = itemId;
    }
}