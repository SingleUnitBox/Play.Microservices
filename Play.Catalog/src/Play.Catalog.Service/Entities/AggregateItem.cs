using Play.Common.SharedKernel;
using Play.Common.SharedKernel.Types;

namespace Play.Catalog.Service.Entities;

public class AggregateItem : AggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public AggregateItem(string name, string description, decimal price)
    {
        Id = new AggregateRootId();
        Name = name;
        Description = description;
        Price = price;
        CreatedDate = DateTimeOffset.UtcNow;
    }
}