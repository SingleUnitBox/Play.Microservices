using System.Linq.Expressions;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Repositories;

public interface IMoneyBagRepository
{
    Task CreateMoneyBag(MoneyBag moneyBag);
    Task UpdateMoneyBag(MoneyBag moneyBag);
    Task<MoneyBag> GetMoneyBagByPlayerId(Guid userId);
}