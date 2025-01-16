using Microsoft.EntityFrameworkCore;

namespace Play.Common.PostgresDb.UnitOfWork;

public abstract class PostgresUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public PostgresUnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task ExecuteAsync(Func<object, Task> action)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            await action(_dbContext);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}