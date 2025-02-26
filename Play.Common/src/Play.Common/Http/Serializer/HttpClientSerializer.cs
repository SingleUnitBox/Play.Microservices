using System.Text.Json;

namespace Play.Common.Http.Serializer;

public class HttpClientSerializer : IHttpClientSerializer
{
    private readonly JsonSerializerOptions _options;

    public HttpClientSerializer(JsonSerializerOptions options = null)
    {
        _options = options ?? new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
    }
    
    public string Serialize<T>(T value)
        => JsonSerializer.Serialize(value, _options);

    public ValueTask<T> DeserializeAsync<T>(Stream stream)
        => JsonSerializer.DeserializeAsync<T>(stream, _options);
}