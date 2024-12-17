namespace Play.Items.Contracts.Commands;

public record CreateItem(string Name, string Description, decimal Price)
{
    public Guid Id { get; } = Guid.NewGuid();
}