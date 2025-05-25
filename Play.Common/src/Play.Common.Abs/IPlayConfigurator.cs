using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Abs;

public interface IPlayConfigurator
{
    IServiceCollection Services { get; }
}