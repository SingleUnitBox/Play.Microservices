﻿using Microsoft.Extensions.Configuration;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public class RabbitMqClient : IRabbitMqClient, IDisposable
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
            _connection = await _connectionFactory.CreateConnectionAsync();
        }

        return _connection;
    }

    public async Task<IChannel> CreateChannel()
    {
        var connection = await GetConnection();
        return await connection.CreateChannelAsync();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}