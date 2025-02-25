using Play.Common.Settings;

namespace Play.Common.Consul.Builders;

internal sealed class ConsulSettingsBuilder : IConsulSettingsBuilder
{
    private readonly ConsulSettings _settings = new();
    
    public IConsulSettingsBuilder Enabled(bool enabled)
    {
        _settings.Enabled = enabled;
        return this;
    }

    public IConsulSettingsBuilder WithUrl(string url)
    {
        _settings.Url = url;
        return this;
    }

    public IConsulSettingsBuilder WithService(string service)
    {
        _settings.Service = service;
        return this;
    }

    public IConsulSettingsBuilder WithAddress(string address)
    {
        _settings.Address = address;
        return this;
    }

    public ConsulSettings Build()
        => _settings;
}