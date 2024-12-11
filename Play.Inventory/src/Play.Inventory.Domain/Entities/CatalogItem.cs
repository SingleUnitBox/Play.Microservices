namespace Play.Inventory.Domain.Entities;

public class CatalogItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public static CatalogItem Create(Guid id, string name, decimal price)
        => new CatalogItem
        {
            Id = id,
            Name = name,
            Price = price
        };
}