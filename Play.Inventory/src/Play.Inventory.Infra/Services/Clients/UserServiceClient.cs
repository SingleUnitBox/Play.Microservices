using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Play.Common.Http;
using Play.Common.Settings;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Services.Clients;

namespace Play.Inventory.Infra.Services.Clients;

internal sealed class UserServiceClient : IUserServiceClient
{
    private readonly IHttpClient _httpClient;
    private readonly string _url;
    private readonly IServiceProvider _serviceProvider;

    public UserServiceClient(
        IHttpClient httpClient,
        HttpClientSettings httpClientSettings, IServiceProvider serviceProvider)
    {
        _httpClient = httpClient;
        _serviceProvider = serviceProvider;
        _url = httpClientSettings.Services["User"];
    }
    public async Task<UserStateDto> GetStateAsync(Guid playerId)
    {
        using var scope = _serviceProvider.CreateScope();
        var clients = scope.ServiceProvider.GetServices<IHttpClient>();
        var response = await _httpClient.GetAsync($"{_url}/user/{playerId}");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var userStateDto = new UserStateDto() { State = responseBody };
        
        return userStateDto;
    }
}