using MassTransit.Topology;

namespace Play.Common.MassTransit.Formatters;

public class EventNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return "Items.ItemCreated";
    }
}