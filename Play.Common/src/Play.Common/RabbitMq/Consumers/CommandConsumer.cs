using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Consumers;

public class CommandConsumer : ICommandConsumer
{
    private readonly IConnection _connection;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IServiceProvider _serviceProvider;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    private readonly IBusPublisher _busPublisher;

    public CommandConsumer(
        IConnection connection,
        ICommandDispatcher commandDispatcher,
        IServiceProvider serviceProvider,
        IExceptionToMessageMapper exceptionToMessageMapper,
        IBusPublisher busPublisher)
    {
        _connection = connection;
        _commandDispatcher = commandDispatcher;
        _serviceProvider = serviceProvider;
        _exceptionToMessageMapper = exceptionToMessageMapper;
        _busPublisher = busPublisher;
    }

    public async Task ConsumeCommand<TCommand>() where TCommand : class, ICommand
    {
        using var channel = await _connection.CreateChannelAsync();

        var queueName = typeof(TCommand).GetQueueName();
        await channel.QueueDeclareAsync(queueName, true, false, false);
        
        var exchangeName = typeof(TCommand).GetExchangeName();
        var routingKey = typeof(TCommand).GetRoutingKey();
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

            var correlationContextAccessor = _serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            correlationContextAccessor.CorrelationContext = 
                new CorrelationContext.CorrelationContext(Guid.Parse(correlationId), userId);
            
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var command = JsonSerializer.Deserialize<TCommand>(message);
            
            try
            {
                await _commandDispatcher.DispatchAsync(command);
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                var rejectedEvent = _exceptionToMessageMapper.Map(e, command);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                await _busPublisher.Publish(rejectedEvent);
            }

        };
        
        await channel.BasicConsumeAsync(queueName, false, consumer);
        await Task.Delay(Timeout.Infinite);
    }
}