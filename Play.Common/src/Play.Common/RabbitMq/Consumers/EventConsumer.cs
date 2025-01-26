using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Consumers;

public class EventConsumer(
    IConnection connection,
    IEventDispatcher eventDispatcher,
    ILogger<EventConsumer> logger,
    IServiceProvider serviceProvider) : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>() where TEvent : class, IEvent
    {
        using var channel = await connection.CreateChannelAsync();

        var queueName = typeof(TEvent).GetQueueName();
        await channel.QueueDeclareAsync(queueName, true, false, false);

        var exchangeName = typeof(TEvent).GetExchangeName();
        var routingKey = typeof(TEvent).GetRoutingKey();
        await channel.QueueBindAsync(queueName, exchangeName, routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
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

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await channel.BasicConsumeAsync(queueName, false, consumer);
        await Task.Delay(Timeout.Infinite);
    }
}