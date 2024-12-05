using Play.Common.Temp.Exceptions;

namespace Play.Catalog.Domain.Exceptions;

public class InvalidPriceException : PlayException
{
    public decimal Price { get; }
    public InvalidPriceException(decimal price) : base($"Price of '{price}' is invalid.")
    {
        Price = price;
    }
}