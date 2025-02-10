using Play.Common.Abs.SharedKernel.Types;

namespace Play.Items.Domain.ValueObjects;

public class ElementId : TypeId
{
    public ElementId(Guid value)
        : base(value)
    {
    }
    
    public static implicit operator ElementId(Guid value) => new(value);
}