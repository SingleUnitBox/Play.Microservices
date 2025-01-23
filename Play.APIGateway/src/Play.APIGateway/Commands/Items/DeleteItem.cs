using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.Items;

public record DeleteItem(Guid ItemId) : ICommand;