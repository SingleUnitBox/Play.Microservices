using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class CannotMakeSocketException : PlayException
{
    public Guid ItemId { get; }

    public string HollowType { get; set; }
    public CannotMakeSocketException(Guid itemId, string hollowType)
        : base($"Cannot make socket '{hollowType}' for the item with id '{itemId}.'")
    {
        ItemId = itemId;
        HollowType = hollowType;
    }
}