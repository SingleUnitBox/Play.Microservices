﻿namespace Play.Common.Http;

public interface IHttpClientSerializer
{
    string Serialize<T>(T value);
    ValueTask<T> DeserializeAsync<T>(Stream stream);
}