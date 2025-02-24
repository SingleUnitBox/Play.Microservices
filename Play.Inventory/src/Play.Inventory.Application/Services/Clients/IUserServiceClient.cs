using Play.Inventory.Application.DTO;

namespace Play.Inventory.Application.Services.Clients;

public interface IUserServiceClient
{
    Task<UserStateDto> GetStateAsync(Guid playerId);
}