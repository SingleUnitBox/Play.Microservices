using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Play.User.Service.Context;

public class Context : IContext
{
    public string RequestId { get; } = Guid.NewGuid().ToString();
    public string TraceId { get; set; }
    public IIdentityContext IdentityContext { get; set; }

    internal Context()
    {
        
    }
    
    public Context(IHttpContextAccessor httpContextAccessor)
        : this(httpContextAccessor.HttpContext.TraceIdentifier,
            new IdentityContext(httpContextAccessor.HttpContext.User))
    {
        
    }
    
    public Context(string traceId, IIdentityContext identityContext)
    {
        TraceId = traceId;
        IdentityContext = identityContext;
    }

    public static Context Empty() => new Context();
}