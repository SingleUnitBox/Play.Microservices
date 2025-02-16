using Microsoft.EntityFrameworkCore;

namespace Play.Common.PostgresDb;

public interface ISeeder
{
    Task SeedAsync(DbContext dbContext);
}