using Play.Common.Consul.Models;

namespace Play.Common.Consul;

public interface IConsulServicesRegistry
{
    Task<ServiceAgent> GetAsync(string name);
}