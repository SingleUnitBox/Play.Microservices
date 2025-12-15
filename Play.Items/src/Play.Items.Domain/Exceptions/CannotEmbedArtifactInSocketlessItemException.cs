using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class CannotEmbedArtifactInSocketlessItemException : PlayException
{
    public Guid ItemId { get; }
    
    public CannotEmbedArtifactInSocketlessItemException(Guid itemId)
        : base($"Cannnot embed artifact in socket-less item with id '{itemId}'.")
    {
        ItemId = itemId;
    }
}