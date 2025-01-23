using System.Text;
using System.Text.Json;
using Play.Common.Abs.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Subscribers;

public class CommandSubscriber : ICommandSubscriber
{
    private readonly IConnection _connection;
    private readonly ICommandDispatcher _commandDispatcher;

    public CommandSubscriber(IConnection connection, ICommandDispatcher commandDispatcher)
    {
        _connection = connection;
        _commandDispatcher = commandDispatcher;
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
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var command = JsonSerializer.Deserialize<TCommand>(message);

                await _commandDispatcher.DispatchAsync(command);
            
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                await channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }

        };
        
        await channel.BasicConsumeAsync(queueName, false, consumer);
        await Task.Delay(Timeout.Infinite);
    }
}