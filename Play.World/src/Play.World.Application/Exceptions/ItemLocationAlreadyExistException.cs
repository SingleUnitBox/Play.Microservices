using Play.Common.Abs.Exceptions;

namespace Play.World.Application.Exceptions;

public class ItemLocationAlreadyExistException : PlayException
{
    public Guid ItemId { get; }
    
    public ItemLocationAlreadyExistException(Guid itemId)
        : base($"Item location for item with id '{itemId}' already exist.")
    {
        ItemId = itemId;
    }
}