namespace Play.Items.Contracts.Events;

public record ItemUpdated(Guid ItemId, string Name, decimal Price);