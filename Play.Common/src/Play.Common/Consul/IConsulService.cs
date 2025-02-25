namespace Play.Common.Consul;

public interface IConsulService
{
    Task<HttpResponseMessage> RegisterServiceAsync(ServiceRegistration registration);
}