using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record CreateItemWithCrafter(string Name, string Description, decimal Price, string CrafterName) : ICommand
{
    public Guid ItemId { get; init; } = Guid.NewGuid();
    public Guid CrafterId { get; init; } = Guid.NewGuid();
}