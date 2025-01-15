using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Abs.SharedKernel;
using Play.Common.Abs.SharedKernel.Types;
using Play.Common.MongoDb.Serializers;
using Play.Common.Settings;
using Play.Common.SharedKernel.Types.Serializers;
using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Helpers;

namespace Play.Items.Tests.Shared.Fixtures;

public class MongoDbFixture<TEntity> : IDisposable where TEntity : AggregateRoot
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<TEntity> _collection;
    
    private static bool _serializersConfigured = true;
    
    public MongoDbFixture()
    {
        MongoDbSerializerConfig.Configure();
        
        var mongoSettings = SettingsHelper.GetSettings<MongoDbSettings>(nameof(MongoDbSettings));
        var serviceSettings = SettingsHelper.GetSettings<ServiceSettings>(nameof(ServiceSettings));
        _client = new MongoClient(mongoSettings.ConnectionString);
        _database = _client.GetDatabase(serviceSettings.ServiceName);
        _collection = _database.GetCollection<TEntity>("items");
    }
    
    public async Task InsertAsync(TEntity entity)
        => await _collection.InsertOneAsync(entity);
    
    public async Task<TEntity> GetAsync(Guid id)
        => await _collection.Find(e => e.Id == new AggregateRootId(id)).SingleOrDefaultAsync();

    public async Task GetAsync(Guid id, TaskCompletionSource<TEntity> tcs)
    {
        var entity = await GetAsync(id);
        if (entity is null)
        {
            tcs.TrySetCanceled();
            return;
        }
        
        tcs.TrySetResult(entity);
    }

    private void ConfigureSerializers()
    {
        if (_serializersConfigured) return; // Skip if already configured

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new AggregateRootIdSerializer());

        _serializersConfigured = true; // Mark as configured
    }
    
    public void Dispose()
    {
        _database.DropCollection("items");
        _client.Dispose();
        Console.WriteLine("Disposing mnogoDb...");
    }
}