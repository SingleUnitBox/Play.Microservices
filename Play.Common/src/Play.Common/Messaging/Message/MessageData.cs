namespace Play.Common.Messaging.Message;

public record MessageData(Guid MessageId, byte[] Payload, string Type);