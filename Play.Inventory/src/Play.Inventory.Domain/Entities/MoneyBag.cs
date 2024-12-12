using Play.Inventory.Domain.Exceptions;

namespace Play.Inventory.Domain.Entities;

public class MoneyBag
{
    public Guid UserId { get; set; }
    public decimal Gold{ get; private set; }

    public void AddGold(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAddedGoldAmountException(amount);
        }

        Gold += amount;
    }
}