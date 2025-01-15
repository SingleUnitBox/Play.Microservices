using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Repositories.Cached.Converters;

internal sealed class ItemJsonConverter : JsonConverter<Item>
{
    public override Item? ReadJson(JsonReader reader, Type objectType, Item? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        
        var name = jsonObject["Name"]?["Value"]?.ToString();
        var description = jsonObject["Description"]?["Value"]?.ToString();
        var price = jsonObject["Price"]?["Value"]?.ToObject<decimal>() ?? 0;
        var createdDate = jsonObject["CreatedDate"]?.ToObject<DateTimeOffset>() ?? DateTimeOffset.UtcNow;
        
        var id = jsonObject["Id"]?["Value"]?.ToObject<Guid>() ?? Guid.Empty;
        
        return new Item(id, name, description, price, createdDate);
    }
    
    public override void WriteJson(JsonWriter writer, Item? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}