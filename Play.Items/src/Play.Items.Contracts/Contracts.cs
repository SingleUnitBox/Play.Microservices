﻿namespace Play.Items.Contracts;

public class Contracts
{
    public record CatalogItemCreated(Guid ItemId, string Name, decimal Price);
    public record CatalogItemUpdated(Guid ItemId, string Name, decimal Price);
    public record CatalogItemDeleted(Guid ItemId);
}