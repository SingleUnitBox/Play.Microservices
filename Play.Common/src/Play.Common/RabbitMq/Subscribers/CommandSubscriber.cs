using System.Text;
using System.Text.Json;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Subscribers;

public class CommandSubscriber : ICommandSubscriber
{
    private readonly IConnection _connection;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    private readonly IBusPublisher _busPublisher;

    public CommandSubscriber(
        IConnection connection,
        ICommandDispatcher commandDispatcher,
        IExceptionToMessageMapper exceptionToMessageMapper,
        IBusPublisher busPublisher)
    {
        _connection = connection;
        _commandDispatcher = commandDispatcher;
        _exceptionToMessageMapper = exceptionToMessageMapper;
        _busPublisher = busPublisher;
    }

    public async Task SubscribeCommand<TCommand>() where TCommand : class, ICommand
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
                // here I can add mapping of rejected events, as it will come from faulty commandHandler
                var rejectedEvent = _exceptionToMessageMapper.Map(e, command);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                await _busPublisher.Publish(rejectedEvent);
            }

        };
        
        await channel.BasicConsumeAsync(queueName, false, consumer);
        await Task.Delay(Timeout.Infinite);
    }
}