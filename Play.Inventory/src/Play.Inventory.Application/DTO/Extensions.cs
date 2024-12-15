using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Application.DTO;

public static  class Extensions
{
    public static ItemDto AsDto(this CatalogItem catalogItem)
        => new ItemDto
        {
            Id = catalogItem.Id,
            Name = catalogItem.Name,
            Price = catalogItem.Price,
        };
}