using System.Diagnostics.Metrics;
using Play.Items.Domain.Types;

namespace Play.Items.Domain.ValueObjects;

public class Socket
{
    public HollowType HollowType { get; }
    
    private Socket(HollowType hollowType)
    {
        HollowType = hollowType;
    }
    
    public static Socket Create(HollowType hollowType)
        => new(hollowType);
    
    public static implicit operator string(Socket socket) => socket.HollowType.ToString();
}