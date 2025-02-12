using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Element
{
    public ElementId ElementId { get; }
    public ElementName ElementName { get; }

    private Element()
    {
    }

    private Element(string elementName)
    {
        ElementId = Guid.NewGuid();
        ElementName = elementName;
    }
    
    public static Element Create(string elementName)
        => new Element(elementName);
}