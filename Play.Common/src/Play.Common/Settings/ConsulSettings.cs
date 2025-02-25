namespace Play.Common.Settings;

public class ConsulSettings
{
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string Service { get; set; }
    public string Address { get; set; }
    public int Port { get; set; }
}