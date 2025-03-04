namespace Play.Common.Settings;

public class RabbitMqSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string VirtualHost { get; init; }
}