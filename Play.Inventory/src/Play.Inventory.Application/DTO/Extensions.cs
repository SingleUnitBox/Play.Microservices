using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Application.DTO;

public static  class Extensions
{
    public static CatalogItemDto AsDto(this CatalogItem catalogItem)
        => new CatalogItemDto
        {
            Id = catalogItem.Id,
            Name = catalogItem.Name,
            Price = catalogItem.Price,
        };
}