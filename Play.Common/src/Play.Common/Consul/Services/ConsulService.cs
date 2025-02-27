using System.Text;
using System.Text.Json;
using Play.Common.Consul.Models;

namespace Play.Common.Consul.Services;

public class ConsulService : IConsulService
{
    private static readonly StringContent EmptyRequest = GetContent(new { });
    private readonly HttpClient _httpClient;
    private const string Version = "v1";

    public ConsulService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<HttpResponseMessage> RegisterServiceAsync(ServiceRegistration registration)
        => _httpClient.PutAsync(GetEndpoint("agent/service/register"), GetContent(registration));

    public Task<HttpResponseMessage> DeregisterServiceAsync(string id)
        => _httpClient.PutAsync(GetEndpoint($"agent/service/deregister/{id}"), EmptyRequest);

    public async Task<IDictionary<string, ServiceAgent>> GetServicesAsync(string service = null)
    {
        var filter = string.IsNullOrWhiteSpace(service) ? string.Empty : $"?filter=Service==\"{service}\"";
        var response = await _httpClient.GetAsync(GetEndpoint($"agent/services{filter}"));
        if (!response.IsSuccessStatusCode)
        {
            return new Dictionary<string, ServiceAgent>();
        }
        
        var content = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<IDictionary<string, ServiceAgent>>(content);
    }

    private static StringContent GetContent(object request)
        => new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
    private static string GetEndpoint(string endpoint) => $"{Version}/{endpoint}";
}