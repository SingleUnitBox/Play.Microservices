using Play.Common.Abs.Exceptions;

namespace Play.Items.Application.Exceptions;

public class CrafterNotFoundException : PlayException
{
    public Guid CrafterId { get; }
    
    public CrafterNotFoundException(Guid crafterId)
        : base($"Crafter with id '{crafterId}' was not found.")
    {
        CrafterId = crafterId;
    }
}