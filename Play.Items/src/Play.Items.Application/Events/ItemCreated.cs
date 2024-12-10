namespace Play.Items.Application.Events;

public class ItemCreated
{
    public Guid ItemId { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
}