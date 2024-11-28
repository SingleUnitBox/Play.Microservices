﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Settings;

namespace Play.Common.MongoDb;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        // guid serializer
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        services.AddSingleton(sp =>
        {
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });
        
        return services;
    }
    
    public static IServiceCollection AddMongoRepository<TEntity>(this IServiceCollection services,
        string collectionName) where TEntity : class, IEntity
    {
        services.AddSingleton<IRepository<TEntity>>(sp =>
        {
            var mongoDb = sp.GetRequiredService<IMongoDatabase>();
            var repository = new MongoRepository<TEntity>(mongoDb, collectionName);

            return repository;
        });
            
        return services;
    }
}