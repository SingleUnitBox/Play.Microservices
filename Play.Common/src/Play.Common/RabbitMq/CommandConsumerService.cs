using System.Reflection;
using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Commands;
using Play.Items.Application.Commands;
using RabbitMQ.Client;

namespace Play.Items.Infra.Consumers;

public class CommandConsumerService : BackgroundService
{
    private readonly CommandConsumer _commandConsumer;
    
    public CommandConsumerService(CommandConsumer commandConsumer)
    {
        _commandConsumer = commandConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandTypes = new[]
        {
            typeof(CreateItem), 
            typeof(UpdateItem), 
            typeof(DeleteItem), 
            typeof(DeleteItems)
        };
        var consumeTasks = commandTypes
            .Select(type =>
            {
                var methodInfo = GetType().GetMethod(nameof(ConsumeGenericCommand), BindingFlags.Instance | BindingFlags.NonPublic);
                if (methodInfo == null)
                {
                    return null;
                }

                var genericMethod = methodInfo.MakeGenericMethod(type);
                return genericMethod.Invoke(this, null) as Task;
            })
            .Where(task => task != null)
            .ToList();
        
        await Task.WhenAll(consumeTasks);
    }

    private Task ConsumeGenericCommand<TCommand>() where TCommand : class, ICommand
    {
        return _commandConsumer.ConsumeCommand<TCommand>();
    }
}