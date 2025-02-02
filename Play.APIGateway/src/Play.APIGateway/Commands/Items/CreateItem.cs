using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.Items;

public record CreateItem(string Name, string Description, Decimal Price, Guid CrafterId) : ICommand;