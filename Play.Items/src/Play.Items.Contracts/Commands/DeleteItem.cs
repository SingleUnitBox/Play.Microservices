using Play.Common.Abs.Commands;

namespace Play.Items.Contracts.Commands;

public record DeleteItem(Guid ItemId) : ICommand;