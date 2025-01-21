using System.Text;
using System.Text.Json;
using Humanizer;
using Play.Common.Abs.Commands;
using Play.Common.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Items.Infra.Consumers;

public class CommandConsumer
{
    private readonly IRabbitMqClient _rabbitMqClient;
    private readonly ICommandDispatcher _commandDispatcher;

    public CommandConsumer(IRabbitMqClient rabbitMqClient,
        ICommandDispatcher commandDispatcher)
    {
        _rabbitMqClient = rabbitMqClient;
        _commandDispatcher = commandDispatcher;
    }

    public async Task ConsumeCommand<TCommand>() where TCommand : class, ICommand
    {
        using var channel = await _rabbitMqClient.CreateChannel();

        var queueName = $"{typeof(TCommand).Name.Underscore()}_queue";
        await channel.QueueDeclareAsync(queueName, true, false, false);
            
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