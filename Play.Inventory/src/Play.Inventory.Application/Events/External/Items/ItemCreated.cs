namespace Play.Inventory.Application.Events.External.Items;

public class ItemCreated
{
    public Guid ItemId { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
}