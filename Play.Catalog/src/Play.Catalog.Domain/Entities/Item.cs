using Play.Catalog.Domain.ValueObjects;
using Play.Common.Abstractions.SharedKernel;
using Play.Common.Abstractions.SharedKernel.Types;

namespace Play.Catalog.Domain.Entities
{
    public class Item : AggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Price Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public Item(string name, string description, decimal price)
        {
            Id = new AggregateRootId();
            Name = name;
            Description = description;
            Price = price;
            CreatedDate = DateTimeOffset.UtcNow;
        }
    }
}