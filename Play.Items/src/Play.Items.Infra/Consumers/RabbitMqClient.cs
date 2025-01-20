using Microsoft.Extensions.Configuration;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Items.Infra.Consumers;

public class RabbitMqClient : IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;

    public RabbitMqClient(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = configuration.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings)).Host,
        };
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is null || !_connection.IsOpen)
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
        }

        return _connection;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        var connection = await GetConnectionAsync();
        return await connection.CreateChannelAsync();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}