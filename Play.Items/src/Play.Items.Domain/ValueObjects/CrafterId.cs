using Play.Common.Abs.SharedKernel.Types;

namespace Play.Items.Domain.ValueObjects;

public class CrafterId : TypeId
{
    public CrafterId(Guid value) : base(value)
    {
    }
    
    public static implicit operator CrafterId(Guid value) => new CrafterId(value);
}