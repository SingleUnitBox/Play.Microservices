namespace Play.Common.Http.Serializer;

public interface IHttpClientSerializer
{
    string Serialize<T>(T value);
    ValueTask<T> DeserializeAsync<T>(Stream stream);
}