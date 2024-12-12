using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Domain.Exceptions;

public class InvalidAddedGoldAmountException : PlayException
{
    public decimal Gold { get; }
    public InvalidAddedGoldAmountException(decimal gold)
        : base($"Added gold amount of '{gold}' is invalid. It must be greater than zero.")
    {
        Gold = gold;
    }
}