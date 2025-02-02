using MongoDB.Driver;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Queries.Handlers;

public class GetItemHandler //: IQueryHandler<GetItem, ItemDto>
{
    // private readonly IMongoCollection<Item> _itemsCollection;
    // private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;
    //
    // public GetItemHandler(IMongoDatabase database)
    // {
    //     _itemsCollection = database.GetCollection<Item>("items");
    // }
    //
    // public async Task<ItemDto> QueryAsync(GetItem query)
    // {
    //     //var filter = _filterBuilder.Eq(i => i.Id.Value, query.ItemId);
    //     var filter = _filterBuilder.Eq("_id", query.ItemId);
    //     var item = await _itemsCollection.Find(filter).SingleOrDefaultAsync();
    //
    //     return item is not null
    //         ? item.AsDto()
    //         : null;
    // }
}