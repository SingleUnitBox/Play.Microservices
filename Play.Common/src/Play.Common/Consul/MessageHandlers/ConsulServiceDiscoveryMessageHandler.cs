using Play.Common.Settings;

namespace Play.Common.Consul.MessageHandlers;

public class ConsulServiceDiscoveryMessageHandler : DelegatingHandler
{
    private readonly IConsulServicesRegistry _servicesRegistry;
    private readonly ConsulSettings _consulSettings;
    private readonly string _serviceName;
    private readonly bool? _overrideRequestUri;

    public ConsulServiceDiscoveryMessageHandler(IConsulServicesRegistry servicesRegistry,
        ConsulSettings consulSettings, string serviceName = null, bool? overrideRequestUri = null)
    {
        _servicesRegistry = servicesRegistry;
        _consulSettings = consulSettings;
        _serviceName = serviceName;
        _overrideRequestUri = overrideRequestUri;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var uri = GetUri(request);
        var serviceName = string.IsNullOrEmpty(_serviceName)
            ? uri.Host
            : _serviceName;

        return await SendAsync(request, serviceName, uri, cancellationToken);
    }

    private Uri GetUri(HttpRequestMessage request)
        => string.IsNullOrWhiteSpace(_serviceName)
            ? request.RequestUri
            : _overrideRequestUri == true
                ? new Uri(
                    $"{request.RequestUri.Scheme}://{_serviceName}/{request.RequestUri.Host}{request.RequestUri.PathAndQuery}")
                : request.RequestUri;

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        string serviceName, Uri uri, CancellationToken cancellationToken)
    {
        if (!_consulSettings.Enabled)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        request.RequestUri = await GetRequestUriAsync(request, serviceName, uri);
        
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<Uri> GetRequestUriAsync(HttpRequestMessage request,
        string serviceName, Uri uri)
    {
        var service = await _servicesRegistry.GetAsync(serviceName);
        if (service is null)
        {
            throw new InvalidOperationException($"Service '{serviceName}' not found");
        }

        var uriBuilder = new UriBuilder(uri)
        {
            //Host = service.Address,
            Host = "localhost",
            Port = service.Port,
        };

        return uriBuilder.Uri;
    }
}