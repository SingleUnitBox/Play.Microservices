namespace Play.Items.Domain.Types;

public enum HollowType
{
    Stone,
    Dust,
    Liquid
}

public class HollowHelper
{
    public static List<string> GetHollowTypes()
    {
        return Enum.GetValues(typeof(HollowType)).Cast<HollowType>().Select(e => e.ToString()).ToList();
    }

    public static bool TryCreateHollowType(
        string hollowTypeString,
        out HollowType hollowType)
        => Enum.TryParse(
               hollowTypeString,
               ignoreCase: true,
               out hollowType)
           && Enum.IsDefined(typeof(HollowType), hollowType);
}