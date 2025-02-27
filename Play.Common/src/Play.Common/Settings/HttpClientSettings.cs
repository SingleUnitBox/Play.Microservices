namespace Play.Common.Settings;

public class HttpClientSettings
{
    public string Type { get; set; }
    public IDictionary<string, string> Services { get; set; }
}