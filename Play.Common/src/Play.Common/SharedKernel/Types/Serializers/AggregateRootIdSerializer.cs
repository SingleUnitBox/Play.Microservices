using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Play.Common.Abstractions.SharedKernel.Types;

namespace Play.Common.SharedKernel.Types.Serializers;

public class AggregateRootIdSerializer : SerializerBase<AggregateRootId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, AggregateRootId id)
    {
        context.Writer.WriteGuid(id.Value);
    }

    public override AggregateRootId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var guid = context.Reader.ReadGuid();
        return new AggregateRootId(guid);
    }
}