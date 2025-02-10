using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class InvalidElementName : PlayException
{
    public string Element { get; }
    
    public InvalidElementName(string element)
        : base($"Element '{element}' is invalid.")
    {
        Element = element;
    }
}