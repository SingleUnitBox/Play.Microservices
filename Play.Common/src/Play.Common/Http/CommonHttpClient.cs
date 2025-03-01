using Play.Common.Http.Serializer;
using Play.Common.Settings;

namespace Play.Common.Http;

public class CommonHttpClient : IHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly HttpClientSettings _settings;
    private readonly IHttpClientSerializer _httpClientSerializer;

    public CommonHttpClient(HttpClient httpClient, HttpClientSettings settings, IHttpClientSerializer httpClientSerializer)
    {
        _httpClient = httpClient;
        _settings = settings;
        _httpClientSerializer = httpClientSerializer;
    }
    
    public virtual Task<HttpResponseMessage> GetAsync(string uri)
        => SendAsync(uri);

    public async Task<T> GetAsync<T>(string uri, IHttpClientSerializer serializer = null)
    {
        var response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStreamAsync();
            return await _httpClientSerializer.DeserializeAsync<T>(content);
        }
        
        return default;
    }

    protected virtual Task<HttpResponseMessage> SendAsync(string uri)
    {
        var requestUri = uri.StartsWith("http") ? uri : $"http://{uri}";

        return _httpClient.GetAsync(requestUri);
    }
}