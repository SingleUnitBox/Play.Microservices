using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Exceptions;

public class ExceptionCompositionRootMapper : IExceptionCompositionRootMapper
{
    private readonly IServiceProvider _serviceProvider;

    public ExceptionCompositionRootMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ExceptionResponse Map(Exception exception)
    {
        using var scope = _serviceProvider.CreateScope();
        var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToList();
        var customMappers = mappers
            .Where(m => m.GetType() != typeof(DefaultExceptionToResponseMapper)).ToList();
        var response = customMappers
            .Select(m => m.Map(exception))
            .SingleOrDefault(r => r is not null);

        if (response is not null)
        {
            return response;
        }
        
        response = mappers.Single(m => m is DefaultExceptionToResponseMapper)
            .Map(exception);
        
        return response;
    }
}