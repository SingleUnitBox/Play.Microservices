using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Queries.Handlers;

public class GetCatalogItemsHandler(ICatalogItemRepository catalogItemRepository) 
    : IQueryHandler<GetCatalogItems, IReadOnlyCollection<ItemDto>>
{
    public async Task<IReadOnlyCollection<ItemDto>> QueryAsync(GetCatalogItems query)
    {
        var items = await catalogItemRepository.BrowseItems();
        return items.Select(i =>
            new ItemDto()
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
            }).ToList();
    }
}