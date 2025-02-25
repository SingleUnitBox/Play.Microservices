using Play.Common.Settings;

namespace Play.Common.Consul.Builders;

public interface IConsulSettingsBuilder
{
    IConsulSettingsBuilder Enabled(bool enabled);
    IConsulSettingsBuilder WithUrl(string url);
    IConsulSettingsBuilder WithService(string service);
    IConsulSettingsBuilder WithAddress(string address);
    ConsulSettings Build();
}