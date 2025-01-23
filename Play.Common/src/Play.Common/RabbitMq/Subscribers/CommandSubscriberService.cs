using System.Reflection;
using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Commands;
using Play.Common.RabbitMq.Consumers;

namespace Play.Common.RabbitMq.Subscribers;

public class CommandSubscriberService : BackgroundService
{
    private readonly CommandConsumer _commandConsumer;
    
    public CommandSubscriberService(CommandConsumer commandConsumer)
    {
        _commandConsumer = commandConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();
        var consumeTasks = commandTypes
            .Select(type =>
            {
                var methodInfo = GetType().GetMethod(nameof(ConsumeGenericCommand), 
                    BindingFlags.Instance | BindingFlags.NonPublic);
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