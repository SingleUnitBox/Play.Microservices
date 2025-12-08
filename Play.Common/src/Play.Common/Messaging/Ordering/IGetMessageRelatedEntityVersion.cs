using Play.Common.Abs.Messaging.Ordering;

namespace Play.Common.Messaging.Ordering;

public interface IGetMessageRelatedEntityVersion<TMessage> where TMessage : IVersionedMessage
{
    Task<int?> GetEntityVersionAsync(TMessage message, CancellationToken cancellationToken = default);
}