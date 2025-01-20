using Play.Common.Abs.Commands;

namespace Play.Items.Contracts.Commands;

public record CreateItem(string Name, string Description, decimal Price) : ICommand
{
    public Guid ItemId { get; init; } = Guid.NewGuid();
}