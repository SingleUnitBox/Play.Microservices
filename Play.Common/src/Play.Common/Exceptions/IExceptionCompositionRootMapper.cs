using Microsoft.AspNetCore.Builder;

namespace Play.Common.Exceptions;

public interface IExceptionCompositionRootMapper
{
    ExceptionResponse Map(Exception exception);
}