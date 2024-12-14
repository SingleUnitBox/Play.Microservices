using Microsoft.EntityFrameworkCore;

namespace Play.Inventory.Infra.Postgres;

public class InventoryPostgresDbContext : DbContext
{
    public InventoryPostgresDbContext() : base()
    {
        
    }
}