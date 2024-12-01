using Microsoft.AspNetCore.Http;

namespace Play.User.Service.Context;

public class ContextFactory : IContextFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IContext Create()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            return new Context(_httpContextAccessor);
        }

        return Context.Empty();
    }
}