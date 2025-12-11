namespace Play.Inventory.Domain.Entities;

public class CatalogItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int LastKnownVersion { get; set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public CatalogItem(Guid id, string name, decimal price, int lastKnownVersion)
    {
        Id = id;
        Name = name;
        Price = price;
        LastKnownVersion = lastKnownVersion;
    }

    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
    }

    public static CatalogItem Create(Guid id, string name, decimal price, int lastKnownVersion)
        => new CatalogItem(id, name, price, lastKnownVersion);
}