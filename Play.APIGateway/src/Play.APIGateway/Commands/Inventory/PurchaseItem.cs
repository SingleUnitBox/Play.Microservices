using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.Inventory;

public record PurchaseItem(Guid PlayerId, Guid ItemId, int Quantity) : ICommand;