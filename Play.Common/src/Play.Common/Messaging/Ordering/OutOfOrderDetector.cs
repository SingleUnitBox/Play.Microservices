using Microsoft.Extensions.Logging;
using Play.Common.Abs.Messaging;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Common.Messaging.Ordering;

public class OutOfOrderDetector(
    Func<Type, object?> currentVersionAccessorFactory,
    ILogger<OutOfOrderDetector> logger)
{
    private readonly Func<Type, object?> _currentVersionAccessorFactory = currentVersionAccessorFactory;
    private readonly ILogger<OutOfOrderDetector>  _logger = logger;

    public async Task<bool> Check<TMessage>(TMessage message) where TMessage : IMessage
    {
        if (message is not IVersionedMessage versionedMessage)
        {
            logger.LogInformation($"'{typeof(TMessage)}' is not versioned, thus cannot be verified.");
        }

        var currentVersion = await GetCurrentVersion(versionedMessage);
        if (currentVersion is null)
        {
            return false;
        }
        
        return currentVersion >= versionedMessage.Version;
    }

    private async Task<int?> GetCurrentVersion<VMessage>(VMessage message) where VMessage : IVersionedMessage
    {
        
    }
}