using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.Items;

public record CreateCrafter(string Name) : ICommand
{
    public Guid ItemId { get; init; } = Guid.NewGuid();
}