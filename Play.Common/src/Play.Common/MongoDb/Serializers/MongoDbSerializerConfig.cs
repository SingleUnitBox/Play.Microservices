using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Play.Common.Abs.SharedKernel.Types;
using Play.Common.SharedKernel.Types.Serializers;

namespace Play.Common.MongoDb.Serializers;

public static class MongoDbSerializerConfig
{
    private static bool _isConfigured = false;

    public static void Configure()
    {
        if (_isConfigured)
        {
            return;
        }
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new AggregateRootIdSerializer());

        _isConfigured = true;
    }
}