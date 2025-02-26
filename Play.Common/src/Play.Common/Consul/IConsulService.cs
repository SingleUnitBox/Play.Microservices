using Play.Common.Consul.Models;

namespace Play.Common.Consul;

public interface IConsulService
{
    Task<HttpResponseMessage> RegisterServiceAsync(ServiceRegistration registration);
    Task<HttpResponseMessage> DeregisterServiceAsync(string id);
}