using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record CreateItem(string Name, string Description, decimal Price, Guid CrafterId, string Element) : ICommand
{
    public Guid ItemId { get; init; } = Guid.NewGuid();
}