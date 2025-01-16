using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class MoneyBagNotFoundException : PlayException
{
    public Guid PlayerId { get; }
    
    public MoneyBagNotFoundException(Guid playerId)
        : base($"Money bag for user with id '{playerId}' was not found.")
    {
        PlayerId = playerId;
    }
}