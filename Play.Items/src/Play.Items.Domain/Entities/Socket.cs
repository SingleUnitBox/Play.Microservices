using Play.Items.Domain.Exceptions;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Socket
{
    public HollowType HollowType { get; }

    public Artifact? Artifact { get; private set; }

    public bool IsOccupied => Artifact is not null;
    
    private Socket(HollowType hollowType)
    {
        HollowType = hollowType;
    }

    public void EmbedArtifact(Artifact artifact)
    {
        if (IsOccupied)
        {
            throw new AlreadyOccupiedHollowException();
        }

        if (artifact.CompatibleHollow != HollowType)
        {
            throw new IncompatibleArtifactException(HollowType.ToString(), artifact.Name);
        }
        
        Artifact = artifact;
    }

    public static Socket Create(HollowType hollowType)
        => new(hollowType);
    
    public static implicit operator string(Socket socket) => socket.HollowType.ToString();
}