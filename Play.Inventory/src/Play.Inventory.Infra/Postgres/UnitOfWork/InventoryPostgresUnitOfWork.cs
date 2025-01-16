using Play.Common.PostgresDb.UnitOfWork;

namespace Play.Inventory.Infra.Postgres.UnitOfWork;

public class InventoryPostgresUnitOfWork : PostgresUnitOfWork<InventoryPostgresDbContext>, IInventoryUnitOfWork
{
    public InventoryPostgresUnitOfWork(InventoryPostgresDbContext dbContext)
        : base(dbContext)
    {
    }
}