namespace Play.Common.Context;

public interface IScopedContext
{
    object CurrentMessage { get; set; }
}