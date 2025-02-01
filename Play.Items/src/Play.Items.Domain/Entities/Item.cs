using Play.Common.Abs.SharedKernel;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities
{
    public class Item : AggregateRoot
    {
        public Name Name { get; set; }
        public Description Description { get; set; }
        public Price Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Crafter Crafter { get; private set; }

        private Item()
        {
            Id = Guid.NewGuid();
        }

        private Item(Guid itemId)
        {
            Id = itemId;
        }
        
        private Item(string name, string description, decimal price, DateTimeOffset createdDate)
            : this()
        {
            Name = name;
            Description = description;
            Price = price;
            CreatedDate = createdDate;
        }
        
        private Item(Guid itemId, string name, string description, decimal price, DateTimeOffset createdDate)
            : this(itemId)
        {
            Name = name;
            Description = description;
            Price = price;
            CreatedDate = createdDate;
        }
        
        private Item(string name, string description, Price price, DateTimeOffset createdDate, Crafter crafter)
            : this(name, description, price, createdDate)
        {
            Crafter = crafter;
        }

        public void SetCrafter(Crafter crafter)
        {
            Crafter = crafter;
        }

        public static Item Create(string name, string description, decimal price, DateTimeOffset createdDate)
            => new(name, description, price, createdDate);
        
        public static Item Create(Guid itemId, string name, string description, decimal price, DateTimeOffset createdDate)
            => new(itemId, name, description, price, createdDate);
        
        public static Item Create(string name, string description, decimal price, DateTimeOffset createdDate, Crafter crafter)
            => new(name, description, price, createdDate, crafter);
    }
}