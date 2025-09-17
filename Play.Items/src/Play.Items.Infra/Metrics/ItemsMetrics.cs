using System.Diagnostics.Metrics;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Metrics;

public class ItemsMetrics
{
    private readonly ObservableGauge<int> _itemsCount;
    
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
    }
}