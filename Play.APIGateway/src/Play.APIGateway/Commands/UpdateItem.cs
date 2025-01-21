using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands;

public record UpdateItem(Guid ItemId, string Name, string Description, Decimal Price) : ICommand;