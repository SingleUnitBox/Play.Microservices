using Play.Items.Domain.Entities;

namespace Play.Items.Domain.Types;

public enum Elements
{
    Water,
    Fire,
    Earth,
    Wind
}

public static class ElementHelper
{
    public static List<string> GetElements()
    {
        return Enum.GetValues(typeof(Elements)).Cast<Elements>().Select(e => e.ToString()).ToList();
    }
}