using Play.Common.Abs.Commands;

namespace Play.Inventory.Contracts.Commands;

public record PurchaseItem(Guid PlayerId, Guid ItemId, int Quantity) : ICommand;