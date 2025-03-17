using System.Text;
using Newtonsoft.Json;

namespace Play.Common.Serialization;

public class JsonSerializer : ISerializer
{
    public byte[] Serialize(object obj)
        => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

    public TMessage Deserialize<TMessage>(string json)
        => JsonConvert.DeserializeObject<TMessage>(json);

    public TMessage DeserializeBinary<TMessage>(byte[] objBytes)
        => JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(objBytes));
}