using Play.Common.Abs.SharedKernel;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities
{
    public class Item : AggregateRoot
    {
        public Name Name { get; set; }
        public Description Description { get; set; }
        public Price Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public Item(string name, string description, Price price)
        {
            Id = new AggregateRootId();
            Name = name;
            Description = description;
            Price = price;
            CreatedDate = DateTimeOffset.UtcNow;
        }
        
        public Item(Guid id, string name, string description, decimal price)
            : this(name, description, price)
        {
            Id = id;
        }
    }
}