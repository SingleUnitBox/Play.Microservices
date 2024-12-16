using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;

namespace Play.Inventory.Application.Queries;

public record GetPlayerInventoryItems(Guid PlayerId) : IQuery<IReadOnlyCollection<InventoryItemDto>>;