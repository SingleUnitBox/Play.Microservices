using System.Linq.Expressions;
using Play.Common.Abs.SharedKernel.Types;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Repositories;

public interface IInventoryItemRepository
{
    Task CreateAsync(InventoryItem item);
    Task UpdateAsync(InventoryItem item);
    Task DeleteAsync(AggregateRootId itemId);
    Task<InventoryItem> GetByIdAsync(AggregateRootId itemId);
    Task<InventoryItem> GetInventory(Expression<Func<InventoryItem, bool>> predicate);
}