using Play.Catalog.Domain.Exceptions;

namespace Play.Catalog.Domain.ValueObjects;

public class Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyNameException();
        }

        Value = value;
    }
    
    public static implicit operator string(Name name) => name.Value;
    public static implicit operator Name(string value) => new Name(value);
}