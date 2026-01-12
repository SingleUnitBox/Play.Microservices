using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.World.Application.Queries;

namespace Play.World.Infrastructure.Postgres.Queries.Handlers;

public class CalculateDistanceHandler(WorldPostgresDbContext dbContext) : IQueryHandler<CalculateDistance, double>
{
    public async Task<double> QueryAsync(CalculateDistance query)
    {
        return await dbContext.Database
            .SqlQuery<double>($@"
            SELECT ST_Distance(
                a.""Position""::geography,
                b.""Position""::geography
            ) AS ""Value""
            FROM ""play.world"".""ItemLocations"" a
            JOIN ""play.world"".""ItemLocations"" b ON b.""ItemId"" = {query.ToItemId}
            WHERE a.""ItemId"" = {query.FromItemId}
        ")
            .FirstAsync();
    }
}