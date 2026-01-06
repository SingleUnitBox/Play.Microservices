using Play.Common.Abs.Exceptions;

namespace Play.World.Infrastructure.Exceptions;

public class WorldExceptionToMessageMapper: IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
        => exception switch
        {
            _ => null
        };
}
