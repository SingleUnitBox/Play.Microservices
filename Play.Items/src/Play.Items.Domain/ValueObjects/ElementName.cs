using Play.Items.Domain.Exceptions;
using Play.Items.Domain.Types;

namespace Play.Items.Domain.ValueObjects;

public struct ElementName
{
    public string Value { get; }

    public ElementName(string value)
    {
        if (!ElementHelper.GetElements().Contains(value))
        {
            throw new InvalidElementName(value);
        }

        Value = value;
    }

    public static implicit operator ElementName(string value) => new(value);
    
    public static bool operator ==(ElementName left, ElementName right)
        => left.Value == right.Value;
    
    public static bool operator !=(ElementName left, ElementName right)
        => !(left == right);
}