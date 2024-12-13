using System.Linq.Expressions;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Repositories;

public interface ICatalogItemRepository
{
    Task CreateAsync(CatalogItem item);
    Task UpdateAsync(CatalogItem item);
    Task DeleteAsync(Guid itemId);
    Task<CatalogItem> GetByIdAsync(Guid itemId);
    Task<CatalogItem> GetAsync(Expression<Func<CatalogItem, bool>> predicate);
    Task<IReadOnlyCollection<CatalogItem>> BrowseItems();
}