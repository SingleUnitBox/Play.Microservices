using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;

namespace Play.Common.PostgresDb.UnitOfWork.Decorators;

[UnitOfWorkDecorator]
public class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler,
        IServiceProvider serviceProvider)
    {
        _commandHandler = commandHandler;
        _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
    }

    public async Task HandleAsync(TCommand command)
    {
        await _unitOfWork.ExecuteAsync(() => _commandHandler.HandleAsync(command));
    }
}