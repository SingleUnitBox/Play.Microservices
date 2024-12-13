using Play.Common.Abs.Commands;

namespace Play.Inventory.Application.Commands;

public record PurchaseItem(Guid UserId, Guid ItemId, int Quantity) : ICommand;