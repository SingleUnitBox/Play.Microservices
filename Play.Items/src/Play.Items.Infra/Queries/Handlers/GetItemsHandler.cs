using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Queries.Handlers;

public class GetItemsHandler : IQueryHandler<GetItems, IEnumerable<ItemDto>>
{
    private readonly IMongoCollection<Item> _itemsCollection;

    public GetItemsHandler(IMongoDatabase database)
    {
        _itemsCollection = database.GetCollection<Item>("items");
    }
    
    public async Task<IEnumerable<ItemDto>> QueryAsync(GetItems query)
    {
        var items = await _itemsCollection.Find(i => true).ToListAsync();
        return items is not null
            ? items.Select(i => i.AsDto()).ToList()
            : new List<ItemDto>();
    }
}