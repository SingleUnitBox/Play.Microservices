namespace Play.Common.Serialization;

public interface ISerializer
{
    byte[] Serialize(object obj);
    TMessage Deserialize<TMessage>(string json);
    object Deserialize(string json, Type type);
    TMessage DeserializeBinary<TMessage>(byte[] objBytes);
}