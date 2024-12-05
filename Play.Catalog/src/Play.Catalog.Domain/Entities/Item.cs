using Play.Catalog.Domain.ValueObjects;
using Play.Common.Temp.SharedKernel;
using Play.Common.Temp.SharedKernel.Types;

namespace Play.Catalog.Domain.Entities
{
    public class Item : AggregateRoot
    {
        public Name Name { get; set; }
        public Description Description { get; set; }
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