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

        public Item(string name, string description, Price price, DateTimeOffset createdDate)
        {
            Id = new AggregateRootId();
            Name = name;
            Description = description;
            Price = price;
            CreatedDate = createdDate;
        }

        public Item(Guid id, string name, string description, decimal price, DateTimeOffset createdDate)
            : this(name, description, price, createdDate)
        {
            Id = id;
        }

        public static Item Create(Guid id, string name, string description, decimal price, DateTimeOffset createdDate)
            => new(id, name, description, price, createdDate);
    }
}