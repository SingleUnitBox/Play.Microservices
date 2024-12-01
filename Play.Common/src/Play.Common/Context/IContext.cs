namespace Play.User.Service.Context;

public interface IContext
{
    string RequestId { get; }
    string TraceId { get; }
    IIdentityContext IdentityContext { get; }
}