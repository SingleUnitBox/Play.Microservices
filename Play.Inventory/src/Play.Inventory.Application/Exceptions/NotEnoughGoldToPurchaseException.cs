using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class NotEnoughGoldToPurchaseException : PlayException
{
    public decimal Gold { get; }
    public int Quantity { get; }
    public string ItemName { get; }
    public NotEnoughGoldToPurchaseException(decimal gold, int quantity, string itemName)
        : base($"'{gold}' gold is not enough to purchase '{quantity}' '{itemName}'.")
    {
        Gold = gold;
        Quantity = quantity;
        ItemName = itemName;
    }
}