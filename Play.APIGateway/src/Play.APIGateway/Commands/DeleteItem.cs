using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands;

public record DeleteItem(Guid ItemId) : ICommand;