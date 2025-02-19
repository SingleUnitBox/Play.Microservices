using System.Text;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq;
using Play.Common.Settings;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Items.Tests.Shared.Fixtures;

public class RabbitMqFixture : IAsyncLifetime
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private IBusPublisher _busPublisher;
    private IConnection _connection; 
    
    public RabbitMqFixture()
    {
        _rabbitMqSettings = SettingsHelper.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
    }

    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class
    {
        await _busPublisher.Publish(message);
    }
    
    public async Task<TaskCompletionSource<TEntity>> SubscribeAndGet<TMessage, TEntity>(
        Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id)
    {
        var tcs = new TaskCompletionSource<TEntity>();
        await using var channel = await _connection.CreateChannelAsync();
        
        var queueName = typeof(TMessage).GetQueueName();
        await channel.QueueDeclareAsync(queueName, true, false, false);
        
        var exchangeName = typeof(TMessage).GetExchangeName();
        var routingKey = typeof(TMessage).GetRoutingKey();
        await channel.QueueBindAsync(queueName, exchangeName, routingKey);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body;
            var json = Encoding.UTF8.GetString(body.Span);
            var message = JsonConvert.DeserializeObject<TEntity>(json);
            
            await onMessageReceived(id, tcs);
        };
        
        await channel.BasicConsumeAsync(queueName, true, consumer);
        
        return tcs;
    }

    public async Task InitializeAsync()
    {
        var factory = new PlayItemsApplicationFactory();
        var scope = factory.Services.CreateScope();
        _busPublisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.Host,
        };
        _connection = await connectionFactory.CreateConnectionAsync();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}