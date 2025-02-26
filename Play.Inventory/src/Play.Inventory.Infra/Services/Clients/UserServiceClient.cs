using System.Net.Http.Headers;
using Newtonsoft.Json;
using Play.Common.Http;
using Play.Common.Settings;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Services.Clients;

namespace Play.Inventory.Infra.Services.Clients;

internal sealed class UserServiceClient : IUserServiceClient
{
    //private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpClient _httpClient;
    private readonly string _url;

    public UserServiceClient(
        IHttpClient httpClient,
        //IHttpClientFactory httpClientFactory,
        HttpClientSettings httpClientSettings)
    {
        _httpClient = httpClient;
        //_httpClientFactory = httpClientFactory;
        _url = httpClientSettings.Services["User"];
    }
    public async Task<UserStateDto> GetStateAsync(Guid playerId)
    {
        //var client = _httpClientFactory.CreateClient();
        var response = await _httpClient.GetAsync($"{_url}/user/{playerId}");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var userStateDto = new UserStateDto() { State = responseBody };
        
        return userStateDto;
    }
}