

namespace Play.Catalog.Application.Commands;

public record CreateItem(string Name, string Description, decimal Price) : ICommand;