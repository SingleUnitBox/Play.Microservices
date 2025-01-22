using Play.Common.Abs.Commands;

namespace Play.Inventory.Application.Commands;

public record PurchaseItem(Guid PlayerId, Guid ItemId, int Quantity) : ICommand;