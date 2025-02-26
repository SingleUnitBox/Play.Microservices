using Play.Common.Http.Serializer;

namespace Play.Common.Http;

public interface IHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<T> GetAsync<T>(string url, IHttpClientSerializer serializer = null);
}