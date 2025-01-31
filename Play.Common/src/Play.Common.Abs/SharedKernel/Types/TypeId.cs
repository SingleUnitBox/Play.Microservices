namespace Play.Common.Abs.SharedKernel.Types;

public abstract class TypeId : IEquatable<TypeId>
{
    public Guid Value { get; }

    public TypeId(Guid value)
    {
        Value = value;
    }
    
    public bool IsEmpty() => Value == Guid.Empty;
    
    public bool Equals(TypeId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TypeId)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static implicit operator Guid(TypeId id) => id.Value;
}