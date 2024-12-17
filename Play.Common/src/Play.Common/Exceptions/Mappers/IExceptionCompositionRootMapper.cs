namespace Play.Common.Exceptions.Mappers;

public interface IExceptionCompositionRootMapper
{
    ExceptionResponse Map(Exception exception);
}