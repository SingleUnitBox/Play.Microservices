﻿using Microsoft.Extensions.DependencyInjection;
using Play.Common.MongoDb;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Repositories;

public static class Extensions
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services.AddMongoRepository<ICatalogItemRepository, CatalogItemRepository>
            (db => new CatalogItemRepository(db, "catalogItems"));
        services.AddMongoRepository<IInventoryItemRepository, InventoryItemRepository>
            (db => new InventoryItemRepository(db, "inventoryItems"));
        services.AddMongoRepository<IPlayerRepository, PlayerRepository>
            (db => new PlayerRepository(db, "users"));
        services.AddMongoRepository<IMoneyBagRepository, MoneyBagRepository>
            (db => new MoneyBagRepository(db, "moneyBags"));
        
        return services;
    }
}