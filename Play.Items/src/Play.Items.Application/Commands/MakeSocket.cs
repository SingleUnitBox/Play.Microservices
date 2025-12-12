using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record MakeSocket(Guid ItemId, string HollowType) : ICommand;