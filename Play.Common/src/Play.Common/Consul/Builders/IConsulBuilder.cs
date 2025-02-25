using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Consul.Builders;

public interface IConsulBuilder
{
    IServiceCollection Services { get; }
    IServiceCollection Build();
}