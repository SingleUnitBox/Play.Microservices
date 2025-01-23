using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Consumers;

public class CommandConsumer : ICommandConsumer
{
    private readonly IConnection _connection;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IServiceProvider _serviceProvider;

    public CommandConsumer(IConnection connection, ICommandDispatcher commandDispatcher, IServiceProvider serviceProvider)
    {
        _connection = connection;
        _commandDispatcher = commandDispatcher;
        _serviceProvider = serviceProvider;
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
            var correlationContextAccessor = _serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            correlationContextAccessor.CorrelationContext = new CorrelationContext.CorrelationContext(Guid.Parse(correlationId));
            
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
                throw;
            }

        };
        
        await channel.BasicConsumeAsync(queueName, false, consumer);
        await Task.Delay(Timeout.Infinite);
    }
}