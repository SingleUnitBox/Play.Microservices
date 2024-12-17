namespace Play.Common.Exceptions.Mappers;

public interface IExceptionToResponseMapper
{
    ExceptionResponse Map(Exception exception);
}