using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Domain.Exceptions;

public class NotEnoughGoldException : PlayException
{
    public decimal Gold { get; }

    public NotEnoughGoldException(decimal gold)
        : base($"Not enough gold to subtract. Only '{gold}' gold available.")
    {
        Gold = gold;
    }
}
