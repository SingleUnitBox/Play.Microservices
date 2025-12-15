using Play.Common.Abs.SharedKernel.Types;

namespace Play.Items.Domain.ValueObjects;

public class SocketId : TypeId
{
    public SocketId(Guid value) : base(value)
    {
    }
    
    public static implicit operator SocketId(Guid value) => new(value);
}