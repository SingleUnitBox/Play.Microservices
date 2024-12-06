using Play.Common.Temp.Exceptions;

namespace Play.Catalog.Application.Exceptions;

public class ItemAlreadyExistException : PlayException
{
    public Guid ItemId { get; }
    public ItemAlreadyExistException(Guid itemId)
        : base($"Item with id '{itemId}' already exists.")
    {
        ItemId = itemId;
    }
}