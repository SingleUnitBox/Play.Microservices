using Microsoft.Extensions.Logging;
using Play.Common.Abs.Messaging;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Common.Messaging.Ordering;

public class OutOfOrderDetector(
    Func<Type, object?> currentVersionAccessorFactory,
    ILogger<OutOfOrderDetector> logger)
{
    public async Task<bool> Check<TMessage>(TMessage message) where TMessage : IMessage
    {
        if (message is not IVersionedMessage versionedMessage)
        {
            logger.LogInformation($"'{typeof(TMessage)}' is not versioned, thus cannot be verified.");
            return false;
        }

        var currentVersion = await GetCurrentVersion(versionedMessage);
        if (currentVersion is null)
        {
            return false;
        }
        
        return currentVersion >= versionedMessage.Version;
    }

    private async Task<int?> GetCurrentVersion<TVerMessage>(TVerMessage message) where TVerMessage : IVersionedMessage
    {
        var getterType = typeof(IGetMessageRelatedEntityVersion<>).MakeGenericType(message.GetType());
        var getter = currentVersionAccessorFactory(getterType);

        if (getter is null)
        {
            logger.LogWarning($"No version getter for the message of type '{typeof(TVerMessage)}' found. Version will not be checked.");
            return null;
        }

        return await (Task<int?>)
            getterType.GetMethod(nameof(IGetMessageRelatedEntityVersion<IVersionedMessage>.GetEntityVersionAsync))
                ?.Invoke(getter, new object[] { message, CancellationToken.None });
    }
}