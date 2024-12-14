using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;

namespace Play.Inventory.Application.Queries;

public record GetUserMoneyBag(Guid PlayerId) : IQuery<UserMoneyBagDto>;