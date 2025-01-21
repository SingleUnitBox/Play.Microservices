using System.Text;
using Humanizer;
using Play.Common.Abs.Commands;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Play.APIGateway.Commands;

public class CommandPublisher
{
    private readonly RabbitMqClient _rabbitMqClient;

    public CommandPublisher(RabbitMqClient rabbitMqClient)
    {
        _rabbitMqClient = rabbitMqClient;
    }
    
    public async Task PublishCommand<TCommand>(TCommand command) where TCommand : ICommand
    {
        using var channel = await _rabbitMqClient.CreateChannelAsync();
        
        //declare exchange
        var exchangeName = $"{command.GetType().Name.Underscore()}_exchange";
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);

        //declare queue
        var queueName = $"{command.GetType().Name.Underscore()}_queue";
        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

        //binding
        var routingKey = command.GetType().Name.Underscore();
        await channel.QueueBindAsync(queueName, exchangeName, routingKey: routingKey);
            
        //serializing
        var message = JsonSerializer.Serialize(command);
        var body = Encoding.UTF8.GetBytes(message);

        //publishing
        await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);
    }
}