using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class InvalidPriceException : PlayException
{
    public decimal Price { get; }
    public InvalidPriceException(decimal price) : base($"Price of '{price}' is invalid.")
    {
        Price = price;
    }
}