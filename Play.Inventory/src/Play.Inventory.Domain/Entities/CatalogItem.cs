namespace Play.Inventory.Domain.Entities;

public class CatalogItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public CatalogItem(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static CatalogItem Create(Guid id, string name, decimal price)
        => new CatalogItem(id, name, price);
}