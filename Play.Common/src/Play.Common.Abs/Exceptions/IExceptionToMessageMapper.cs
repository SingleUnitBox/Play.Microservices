namespace Play.Common.Abs.Exceptions;

public interface IExceptionToMessageMapper
{
    object Map(Exception exception, object message);
}