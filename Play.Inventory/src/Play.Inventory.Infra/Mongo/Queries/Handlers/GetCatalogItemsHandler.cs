using MongoDB.Driver;
using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Mongo.Queries.Handlers;

public class GetCatalogItemsHandler //: IQueryHandler<GetCatalogItems, IReadOnlyCollection<ItemDto>>
{
    private readonly IMongoDatabase _mongoDatabase;

    public GetCatalogItemsHandler(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }
    public async Task<IReadOnlyCollection<ItemDto>> QueryAsync(GetCatalogItems query)
    {
        var items = await _mongoDatabase
            .GetCollection<CatalogItem>("inventoryItems")
            .Find(i => true).ToListAsync();

        return items.Select(i => i.AsDto()).ToList();
    }
}
