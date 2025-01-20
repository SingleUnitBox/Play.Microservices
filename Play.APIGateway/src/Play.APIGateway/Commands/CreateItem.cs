using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands;

public record CreateItem(string Name, string Description, Decimal Price) : ICommand;