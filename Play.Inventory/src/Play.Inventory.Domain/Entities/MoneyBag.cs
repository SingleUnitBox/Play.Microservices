using Play.Inventory.Domain.Exceptions;

namespace Play.Inventory.Domain.Entities;

public class MoneyBag
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; private set; }
    public decimal Gold { get; private set; }

    private MoneyBag(Guid playerId) : this(playerId, 100)
    {

    }
    
    private MoneyBag(Guid playerId, decimal gold)
    {
        PlayerId = playerId;
        Gold = gold;
    }
    
    public void AddGold(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAddedGoldAmountException(amount);
        }

        Gold += amount;
    }

    public void SubtractGold(decimal amount)
    {
        if (Gold < amount)
        {
            throw new NotEnoughGoldException(Gold);
        }
        
        Gold -= amount;
    }
    
    public static MoneyBag Create(Guid playerId)
        => new MoneyBag(playerId);
}