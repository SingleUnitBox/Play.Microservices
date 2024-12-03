using Play.Catalog.Domain.Entities;

namespace Play.Catalog.Application.DTO
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
            => new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
}