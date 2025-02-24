using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Infra.Exceptions;

internal sealed class InventoryExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
        => exception switch
        {
            _ => null
        };
}