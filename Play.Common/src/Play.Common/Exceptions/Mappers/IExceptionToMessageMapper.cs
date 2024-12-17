namespace Play.Common.Exceptions.Mappers;

public interface IExceptionToMessageMapper
{
    object Map(Exception exception, object message);
}