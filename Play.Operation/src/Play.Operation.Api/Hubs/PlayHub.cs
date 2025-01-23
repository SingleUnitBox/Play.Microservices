using Microsoft.AspNetCore.SignalR;
using Play.Operation.Api.Infrastructure;

namespace Play.Operation.Api.Hubs;

public class PlayHub : Hub
{
    public async Task InitializeAsync(string userId)
    {
            if (string.IsNullOrWhiteSpace(userId))
        {
            await DisconnectAsync();
        }
        try
        {
            var group = Guid.Parse(userId).ToUserGroup();
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await ConnectAsync();
        }
        catch
        {
            await DisconnectAsync();
        }
    }
    
    // refactor to pull userId from jwtToken
    // public async Task InitializeAsync(string token)
    // {
    //     if (string.IsNullOrWhiteSpace(token))
    //     {
    //         await DisconnectAsync();
    //     }
    //     try
    //     {
    //         var payload = _jwtHandler.GetTokenPayload(token);
    //         if (payload is null)
    //         {
    //             await DisconnectAsync();
    //                 
    //             return;
    //         }
    //
    //         var group = Guid.Parse(payload.Subject).ToUserGroup();
    //         await Groups.AddToGroupAsync(Context.ConnectionId, group);
    //         await ConnectAsync();
    //     }
    //     catch
    //     {
    //         await DisconnectAsync();
    //     }
    // }

    private async Task ConnectAsync()
    {
        await Clients.Client(Context.ConnectionId).SendAsync("connected");
    }

    private async Task DisconnectAsync()
    {
        await Clients.Client(Context.ConnectionId).SendAsync("disconnected");
    }
}