using Play.Common.Abs.SharedKernel;
using Play.Items.Domain.DomainEvents;
using Play.Items.Domain.Exceptions;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities
{
    public class Item : AggregateRoot
    {
        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public Price Price { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }
        public Crafter Crafter { get; private set; }
        public Element Element { get; private set; }
        
        public Socket? Socket { get; private set; }

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
            AddEvent(new ItemCreated(Id, Name, Price));
        }
        
        private Item(string name, string description, Price price, DateTimeOffset createdDate, Crafter crafter)
            : this(name, description, price, createdDate)
        {
            Crafter = crafter;
            AddEvent(new ItemCreated(Id, Name, Price));
        }

        public void UpdateName(Name newName)
        {
            Name = newName;
            AddEvent(new NameUpdated(this));
        }
        
        public void UpdateDescription(Description newDescription)
        {
            Description = newDescription;
            AddEvent(new DescriptionUpdated(this));
        }

        public void UpdatePrice(Price newPrice)
        {
            Price = newPrice;
            AddEvent(new PriceUpdated(this));
        }

        public void SetCrafter(Crafter crafter)
        {
            Crafter = crafter;
        }

        public void SetElement(Element element)
        {
            Element = element;
        }

        public void MakeSocket(Socket socket)
        {
            if (Socket is not null)
            {
                throw new CannotMakeSocketException(Id, socket);
            }

            Socket = socket;
            AddEvent(new SocketCreated(this));
        }

        public void Delete()
        {
            AddEvent(new ItemDeleted(Id));
        }

        public static Item Create(string name, string description, decimal price, DateTimeOffset createdDate)
            => new(name, description, price, createdDate);

        public static Item Create(Guid itemId, string name, string description, decimal price,
            DateTimeOffset createdDate)
            => new Item(itemId, name, description, price, createdDate);
    }
}