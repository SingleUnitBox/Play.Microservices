using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class CatalogItemNotFoundException : PlayException
{
    public Guid ItemId { get; }
    public CatalogItemNotFoundException(Guid itemId)
        : base($"Catalog item with id '{itemId}' was not found.")
    {
        ItemId = itemId;
    }
}