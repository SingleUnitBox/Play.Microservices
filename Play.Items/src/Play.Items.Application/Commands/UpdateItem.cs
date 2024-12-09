using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record UpdateItem(Guid ItemId, string Name, string Description, decimal Price) : ICommand;