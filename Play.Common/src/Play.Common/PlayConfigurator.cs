using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs;

namespace Play.Common;

internal sealed class PlayConfigurator : IPlayConfigurator
{ 
    public IServiceCollection Services { get; }

    public PlayConfigurator(IServiceCollection services)
    {
        Services = services;
    }
}