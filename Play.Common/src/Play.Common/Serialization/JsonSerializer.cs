using System.Text;

namespace Play.Common.Serialization;

public class JsonSerializer : ISerializer
{
    public byte[] Serialize(object obj)
        => Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(obj));

    public TMessage Deserialize<TMessage>(string json)
        => System.Text.Json.JsonSerializer.Deserialize<TMessage>(json);

    public object Deserialize(string json, Type type)
        => System.Text.Json.JsonSerializer.Deserialize(json, type);

    public TMessage DeserializeBinary<TMessage>(byte[] objBytes)
        => System.Text.Json.JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(objBytes));
}