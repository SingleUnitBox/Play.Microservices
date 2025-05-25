using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs;

namespace Play.Common;

public static class Extensions
{
    public static IPlayConfigurator AddPlayMicroservice(this IServiceCollection services,
        IConfiguration configuration,
        Action<IPlayConfigurator> configure = default)
    {
        var playConfigurator = new PlayConfigurator(services);
        configure?.Invoke(playConfigurator);
        
        return playConfigurator;
    }
}