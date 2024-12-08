using Play.Common.Temp.Commands;

namespace Play.Catalog.Application.Commands;

public record DeleteItem(Guid ItemId) : ICommand;