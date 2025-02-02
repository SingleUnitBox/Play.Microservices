using Play.Common.Abs.SharedKernel;
using Play.Items.Domain.DomainEvents;
using Play.Items.Domain.Exceptions;
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
            UpdateName(name);
            UpdateDescription(description);
            UpdatePrice(price);
            CreatedDate = createdDate;
            AddEvent(new ItemCreated(Id, Name, Price));
        }
        
        private Item(string name, string description, Price price, DateTimeOffset createdDate, Crafter crafter)
            : this(name, description, price, createdDate)
        {
            Crafter = crafter;
        }

        public void UpdateName(Name newName)
        {
            if (string.IsNullOrWhiteSpace(newName.Value))
            {
                throw new EmptyNameException();
            }

            Name = newName;
            AddEvent(new NameUpdated(Id, Name, Price));
        }
        
        public void UpdateDescription(Description newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new EmptyDescriptionException();
            }

            Description = newDescription;
            AddEvent(new DescriptionUpdated(Id, Name, Price));
        }

        public void UpdatePrice(Price newPrice)
        {
            if (newPrice < 0)
            {
                throw new InvalidPriceException(newPrice);
            }

            Price = newPrice;
            AddEvent(new PriceUpdated(Id, Name, Price));
        }

        public void SetCrafter(Crafter crafter)
        {
            Crafter = crafter;
        }

        public static Item Create(string name, string description, decimal price, DateTimeOffset createdDate)
            => new(name, description, price, createdDate);

        public static Item Create(Guid itemId, string name, string description, decimal price,
            DateTimeOffset createdDate)
            => new Item(itemId, name, description, price, createdDate);
                
        public static Item Create(string name, string description, decimal price, DateTimeOffset createdDate, Crafter crafter)
            => new(name, description, price, createdDate, crafter);
    }
}