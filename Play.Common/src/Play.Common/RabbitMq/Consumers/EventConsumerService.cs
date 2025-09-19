using System.Reflection;
using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Events;

namespace Play.Common.RabbitMq.Consumers;

public class EventConsumerService(IEventConsumer eventConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();
        
        var methodInfo = GetType().GetMethod(nameof(ConsumeGenericEvent),
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (methodInfo is null)
        {
            return;
        }
        
        var consumeTasks = eventTypes
            .Select(t =>
            {
                var genericMethod = methodInfo.MakeGenericMethod(t);
                return genericMethod?.Invoke(this, new object[] { stoppingToken }) as Task;
            })
            .Where(task => task is not null)
            .ToList();
        
        await Task.WhenAll(consumeTasks);
    }
    
    private Task ConsumeGenericEvent<TEvent>(CancellationToken stoppingToken) where TEvent : class, IEvent
        => eventConsumer.ConsumeEvent<TEvent>(stoppingToken);
}