using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Settings;
using Play.Common.SharedKernel.Types.Serializers;

namespace Play.Common.MongoDb;

public static class Extensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new AggregateRootIdSerializer());
        
        services.AddSingleton(sp =>
        {
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });
        
        return services;
    }
    
    public static IServiceCollection AddMongoDbWithMongoClient(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new AggregateRootIdSerializer());

        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(configuration
                .GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()?.ConnectionString));
        
        services.AddSingleton(sp =>
        {
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });
        
        return services;
    }
    
    // public static IServiceCollection AddMongoRepository<TEntity>(this IServiceCollection services,
    //     string collectionName) where TEntity : class, IEntity
    // {
    //     services.AddSingleton<IRepository<TEntity>>(sp =>
    //     {
    //         var mongoDb = sp.GetRequiredService<IMongoDatabase>();
    //         var repository = new MongoRepository<TEntity>(mongoDb, collectionName);
    //
    //         return repository;
    //     });
    //         
    //     return services;
    // }
    
    public static IServiceCollection AddMongoRepository<TIMongoRepository, TMongoRepository>(
        this IServiceCollection services,
        Func<IMongoDatabase, TMongoRepository> factory)
        where TIMongoRepository : class
        where TMongoRepository : TIMongoRepository
    {
        services.AddScoped<TIMongoRepository>(sp =>
        {
            var mongoDb = sp.GetRequiredService<IMongoDatabase>();
            return factory(mongoDb);
        });
        
        return services;
    }
}