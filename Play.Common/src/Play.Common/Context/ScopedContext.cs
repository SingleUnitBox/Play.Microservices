namespace Play.Common.Context;

public class ScopedContext : IScopedContext
{
    public object CurrentMessage { get; set; }
}