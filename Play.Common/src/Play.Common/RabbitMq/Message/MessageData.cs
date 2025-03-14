namespace Play.Common.RabbitMq.Message;

public record MessageData(Guid MessageId, byte[] Payload, string Type);