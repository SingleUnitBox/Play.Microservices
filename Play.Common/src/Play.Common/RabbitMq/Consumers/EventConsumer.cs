using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Consumers;

internal sealed class EventConsumer(
    ChannelFactory channelFactory,
    IEventDispatcher eventDispatcher,
    ILogger<EventConsumer> logger,
    IServiceProvider serviceProvider) : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>(CancellationToken stoppingToken) where TEvent : class, IEvent
    {
        using var channel = channelFactory.CreateForConsumer();

        var queueName = typeof(TEvent).GetQueueName();
        channel.QueueDeclare(queueName, true, false, false);

        var exchangeName = typeof(TEvent).GetExchangeName();
        var routingKey = typeof(TEvent).GetRoutingKey();
        channel.QueueBind(queueName, exchangeName, routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var correlationId = ea.BasicProperties?.CorrelationId ?? Guid.Empty.ToString();
            var userIdString = string.Empty;
            if (ea.BasicProperties?.Headers?.TryGetValue("UserId", out var userIdHeader) == true &&
                userIdHeader is byte[] userIdBytes)
            {
                userIdString = Encoding.UTF8.GetString(userIdBytes);
            }
            
            var userId = Guid.TryParse(userIdString, out var userIdGuid)
                ? userIdGuid
                : Guid.Empty;
            
            var correlationContextAccessor = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            correlationContextAccessor.CorrelationContext = 
                new CorrelationContext.CorrelationContext(Guid.Parse(correlationId), userId);
            
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TEvent>(message);

                await eventDispatcher.HandleAsync(@event);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                channel.BasicAck(ea.DeliveryTag, false);
            }
        };

        channel.BasicConsume(queueName, false, consumer);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {

        }
        finally
        {
            if (channel.IsOpen)
            {
                channel.Close();
                channel.Dispose();
            }
        }
    }
}