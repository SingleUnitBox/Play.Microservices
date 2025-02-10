using Microsoft.EntityFrameworkCore;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Postgres.Repositories;

internal sealed class ElementRepository : IElementRepository
{
    private readonly DbSet<Element> _elements;
    private readonly ItemsPostgresDbContext _dbContext;

    public ElementRepository(ItemsPostgresDbContext dbContext)
    {
        _elements = dbContext.Elements;
        _dbContext = dbContext;
    }

    public async Task AddElement(Element element)
    {
        await _elements.AddAsync(element);
        await _dbContext.SaveChangesAsync();
    }

    public Task<Element> GetElement(Guid elementId)
        => _elements.SingleOrDefaultAsync(e => e.ElementId == elementId);
}