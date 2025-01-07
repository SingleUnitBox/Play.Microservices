using Play.Items.Domain.Exceptions;

namespace Play.Items.Domain.ValueObjects;

public class Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0 || value > 1_000)
        {
            throw new InvalidPriceException(value);
        }

        Value = value;
    }

    public static implicit operator decimal(Price price) => price.Value;
    public static implicit operator Price(decimal price) => new Price(price);
}