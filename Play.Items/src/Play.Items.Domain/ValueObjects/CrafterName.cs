using Play.Items.Domain.Exceptions;

namespace Play.Items.Domain.ValueObjects;

public class CrafterName
{
    public string Value { get; }

    public CrafterName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidCrafterNameException();
        }
        
        Value = value;
    }
    
    public static implicit operator CrafterName(string value) => new CrafterName(value);
    public static implicit operator string(CrafterName value) => value.Value;
}