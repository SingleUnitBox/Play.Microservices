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
    
    public Task<HttpResponseMessage> GetAsync(string url)
        => _httpClient.GetAsync(url);

    public async Task<T> GetAsync<T>(string url, IHttpClientSerializer serializer = null)
    {
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStreamAsync();
            return await _httpClientSerializer.DeserializeAsync<T>(content);
        }
        
        return default;
    }
}