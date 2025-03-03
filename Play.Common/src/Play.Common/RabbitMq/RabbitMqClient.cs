using Microsoft.Extensions.Configuration;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public class RabbitMqClient : IRabbitMqClient, IAsyncDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;

    public RabbitMqClient(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = configuration.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings)).Host,
            ClientProvidedName = configuration.GetSettings<ServiceSettings>(nameof(ServiceSettings)).ServiceName
        };
    }

    public async Task<IConnection> GetConnection()
    {
        if (_connection is null || !_connection.IsOpen)
        {
            _connection = await _connectionFactory.CreateConnection();
        }

        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null && _connection.IsOpen)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
    }
}