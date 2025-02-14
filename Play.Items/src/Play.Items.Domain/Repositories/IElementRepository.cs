using Play.Items.Domain.Entities;

namespace Play.Items.Domain.Repositories;

public interface IElementRepository
{
    Task AddElement(Element element);
    Task<Element> GetElement(string elementName);
}