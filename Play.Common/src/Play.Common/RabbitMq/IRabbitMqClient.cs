using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public interface IRabbitMqClient
{
    Task<IConnection> GetConnection();
    Task<IChannel> CreateChannel();
}