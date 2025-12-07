namespace Play.Common.Abs.Messaging.Ordering;

public interface IVersionedMessage : IMessage
{
    int Version { get; }
}