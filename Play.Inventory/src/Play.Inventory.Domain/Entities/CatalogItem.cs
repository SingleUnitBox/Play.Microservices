namespace Play.Inventory.Domain.Entities;

public class CatalogItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}