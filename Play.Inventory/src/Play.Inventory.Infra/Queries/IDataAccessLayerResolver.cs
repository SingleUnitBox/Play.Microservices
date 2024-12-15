using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Queries;

public interface IDataAccessLayerResolver
{
    Task<IReadOnlyCollection<CatalogItem>> Resolve();
}