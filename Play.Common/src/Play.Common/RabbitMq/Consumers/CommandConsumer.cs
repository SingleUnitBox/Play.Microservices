using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Consumers;

internal sealed class CommandConsumer : ICommandConsumer
{
    private readonly ChannelFactory _channelFactory;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IServiceProvider _serviceProvider;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    private readonly IBusPublisher _busPublisher;

    public CommandConsumer(
        ChannelFactory channelFactory,
        ICommandDispatcher commandDispatcher,
        IServiceProvider serviceProvider,
        IExceptionToMessageMapper exceptionToMessageMapper,
        IBusPublisher busPublisher)
    {
        _channelFactory = channelFactory;
        _commandDispatcher = commandDispatcher;
        _serviceProvider = serviceProvider;
        _exceptionToMessageMapper = exceptionToMessageMapper;
        _busPublisher = busPublisher;
    }

    public async Task ConsumeCommand<TCommand>(CancellationToken stoppingToken) where TCommand : class, ICommand
    {
        var channel = _channelFactory.CreateForConsumer();

        var queueName = typeof(TCommand).GetQueueName();
        channel.QueueDeclare(queueName, true, false, false);
        
        var exchangeName = typeof(TCommand).GetExchangeName();
        var routingKey = typeof(TCommand).GetRoutingKey();
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

            var correlationContextAccessor = _serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            correlationContextAccessor.CorrelationContext = 
                new CorrelationContext.CorrelationContext(Guid.Parse(correlationId), userId);
            
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var command = JsonSerializer.Deserialize<TCommand>(message);
            
            try
            {
                await _commandDispatcher.DispatchAsync(command);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                var rejectedEvent = _exceptionToMessageMapper.Map(e, command);
                channel.BasicAck(ea.DeliveryTag, false);
                await _busPublisher.Publish(rejectedEvent);
            }

        };
        
        channel.BasicConsume(queueName, false, consumer);
    }
}