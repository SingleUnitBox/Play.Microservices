using Play.Common.Temp.Commands;

namespace Play.Catalog.Application.Commands;

public record UpdateItem(Guid ItemId, string Name, string Description, decimal Price) : ICommand;