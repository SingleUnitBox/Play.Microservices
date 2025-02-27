using Play.Common.Http;
using Play.Common.Http.Serializer;
using Play.Common.Settings;

namespace Play.Common.Consul.Http;

public class ConsulHttpClient : CommonHttpClient, IConsulHttpClient
{
    public ConsulHttpClient(HttpClient httpClient, HttpClientSettings settings, IHttpClientSerializer httpClientSerializer)
        : base(httpClient, settings, httpClientSerializer)
    {
    }
}