using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record CreateCrafter(string Name) : ICommand
{
    public Guid CrafterId { get; init; } = Guid.NewGuid();
}