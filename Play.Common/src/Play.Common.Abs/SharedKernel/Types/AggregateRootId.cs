namespace Play.Common.Abs.SharedKernel.Types
{
    public class AggregateRootId<TKey> : IEquatable<AggregateRootId<TKey>>
    {
        public TKey Value { get; }

        public AggregateRootId(TKey value)
            => Value = value;

        public bool Equals(AggregateRootId<TKey> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TKey>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((AggregateRootId<TKey>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Value);
        }
    }

    public class AggregateRootId : AggregateRootId<Guid>
    {
        public AggregateRootId() : base(Guid.NewGuid())
        {
            
        }

        public AggregateRootId(Guid value) : base(value)
        {
            
        }

        public static implicit operator Guid(AggregateRootId id) => id.Value;
        public static implicit operator AggregateRootId(Guid value) => new AggregateRootId(value);

        public override string ToString()
            => Value.ToString();       
    }
}
