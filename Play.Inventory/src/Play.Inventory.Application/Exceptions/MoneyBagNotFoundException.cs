using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class MoneyBagNotFoundException : PlayException
{
    public Guid UserId { get; }
    
    public MoneyBagNotFoundException(Guid userId)
        : base($"Money bag for user with id '{userId}' was not found.")
    {
        UserId = userId;
    }
}