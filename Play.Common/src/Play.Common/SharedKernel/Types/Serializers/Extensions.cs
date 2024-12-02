using MongoDB.Bson.Serialization;

namespace Play.Common.SharedKernel.Types.Serializers;

public static class Extensions
{
    public static void AddAggregateRootIdSerializer()
    {
        BsonSerializer.RegisterSerializer(new AggregateRootIdSerializer());
    }
}