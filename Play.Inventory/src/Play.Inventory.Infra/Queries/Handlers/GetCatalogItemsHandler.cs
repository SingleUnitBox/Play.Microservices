using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Queries;
using Play.Common.Settings;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Infra.Mongo.Queries.Handlers;

namespace Play.Inventory.Infra.Queries.Handlers;

public class GetCatalogItemsHandler : IQueryHandler<GetCatalogItems, IReadOnlyCollection<ItemDto>>
{
    private readonly IDataAccessLayerResolver _dataAccessLayerResolver;

    public GetCatalogItemsHandler(IDataAccessLayerResolver dataAccessLayer)
    {
        _dataAccessLayerResolver = dataAccessLayer;
    }
    public async Task<IReadOnlyCollection<ItemDto>> QueryAsync(GetCatalogItems query)
    {
        var items = await _dataAccessLayerResolver.Resolve();
        return items.Select(i =>
            new ItemDto()
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
            }).ToList();
    }
}