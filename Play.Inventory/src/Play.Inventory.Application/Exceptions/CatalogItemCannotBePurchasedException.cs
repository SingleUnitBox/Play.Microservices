using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class CatalogItemCannotBePurchasedException : PlayException
{
    public Guid CatalogItemId { get; }
    public string CatalogItemName { get; }
    public CatalogItemCannotBePurchasedException(Guid catalogItemId, string catalogItemName)
        : base($"Catalog item '{catalogItemName}' with id '{catalogItemId}' cannot be purchased.")
    {
        CatalogItemId = catalogItemId;
        CatalogItemName = catalogItemName;
    }
}