using System.Text;
using System.Text.Json;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Items.Application.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Items.Infra.Consumers;

public class CommandConsumer
{
    private readonly RabbitMqClient _rabbitMqClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandDispatcher _commandDispatcher;

    public CommandConsumer(RabbitMqClient rabbitMqClient,
        IServiceProvider serviceProvider,
        ICommandDispatcher commandDispatcher)
    {
        _rabbitMqClient = rabbitMqClient;
        _serviceProvider = serviceProvider;
        _commandDispatcher = commandDispatcher;
    }

    public async Task ConsumeCommand<TCommand>() where TCommand : class, ICommand
    {
        using var channel = await _rabbitMqClient.CreateChannelAsync();

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