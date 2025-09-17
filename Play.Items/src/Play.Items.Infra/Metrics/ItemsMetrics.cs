using System.Diagnostics.Metrics;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Metrics;

public class ItemsMetrics
{
    private readonly ObservableGauge<int> _itemsCount;
    private readonly Counter<long> _queryCounter;
    private readonly Counter<long> _commandCounter;
    
    public ItemsMetrics(IMeterFactory meterFactory, IItemRepository itemRepository)
    {
        var meter = meterFactory.Create("play.items.meter");
        _itemsCount = meter.CreateObservableGauge<int>(
            "play.items.items_count",
            () =>
            {
                var count = itemRepository.Count();
                return new Measurement<int>(count);
            },
            unit: "{items}",
            description: "Current amount of items in the Play.Items service.");
        
        _queryCounter = meter.CreateCounter<long>(
            "play.items.queries_total",
            description: "Total number of queries in the Play.Items service.");
        
        _commandCounter = meter.CreateCounter<long>(
            "play.items.commands_total",
            description: "Total number of commands in the Play.Items service.");
    }

    public void IncrementQuery() => _queryCounter.Add(1);
    
    public void IncrementCommand() => _queryCounter.Add(1);
}