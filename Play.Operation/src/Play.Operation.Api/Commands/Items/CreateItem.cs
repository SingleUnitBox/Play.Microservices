using Play.Common.Abs.Commands;

namespace Play.Operation.Api.Commands.Items;

public record CreateItem(string Name, string Description, decimal Price) : ICommand
{
    public Guid ItemId { get; init; } = Guid.NewGuid();
}