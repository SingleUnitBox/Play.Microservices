using Play.Common.Abs.Exceptions;

namespace Play.Items.Application.Exceptions;

public class CrafterAlreadyExistsException : PlayException
{
    public Guid CrafterId { get; }
    
    public CrafterAlreadyExistsException(Guid crafterId)
        : base($"Crafter with id '{crafterId}' already exists.")
    {
        CrafterId = crafterId;
    }
}