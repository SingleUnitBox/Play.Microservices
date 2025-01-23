namespace Play.Common.PostgresDb.UnitOfWork;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}