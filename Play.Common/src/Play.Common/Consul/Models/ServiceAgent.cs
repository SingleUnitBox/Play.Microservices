namespace Play.Common.Consul.Models;

public class ServiceAgent
{
    public string Id { get; set; }
    public string Service { get; set; }
    public int Port { get; set; }
    public string Address { get; set; }
}