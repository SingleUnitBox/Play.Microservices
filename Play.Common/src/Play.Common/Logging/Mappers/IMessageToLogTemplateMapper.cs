namespace Play.Common.Logging.Mappers;

public interface IMessageToLogTemplateMapper
{
    HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class;
}