namespace Play.Common.Common;

public class ServiceId : IServiceId
{
    public string Id { get; } = $"{Guid.NewGuid():N}";
}