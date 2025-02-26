

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

    private static StringContent GetContent(object request)
        => new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
    private static string GetEndpoint(string endpoint) => $"{Version}/{endpoint}";
}