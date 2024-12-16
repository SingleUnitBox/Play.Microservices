using Play.Inventory.Application.DTO;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Queries.Handlers;

public interface IDataAccessLayer
{
    Task<IReadOnlyCollection<CatalogItem>> BrowseItems();
    
}