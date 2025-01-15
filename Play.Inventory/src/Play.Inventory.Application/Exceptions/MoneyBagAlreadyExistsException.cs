using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class MoneyBagAlreadyExistsException : PlayException
{
    private readonly Guid PlayerId;
    public MoneyBagAlreadyExistsException(Guid playerId)
        : base($"Money bag for player with id '{playerId}' already exists.")
    {
        PlayerId = playerId;
    }
}