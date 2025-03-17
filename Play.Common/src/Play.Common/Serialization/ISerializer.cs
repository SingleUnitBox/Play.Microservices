namespace Play.Common.Serialization;

public interface ISerializer
{
    byte[] Serialize(object obj);
    TMessage Deserialize<TMessage>(string json);
    TMessage DeserializeBinary<TMessage>(byte[] objBytes);
}