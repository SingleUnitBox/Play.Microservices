using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.Items;

public record MakeSocket(Guid ItemId, string HollowType) : ICommand;