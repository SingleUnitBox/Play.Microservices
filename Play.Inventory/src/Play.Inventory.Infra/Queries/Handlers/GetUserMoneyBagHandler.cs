using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Queries.Handlers;

public class GetUserMoneyBagHandler : IQueryHandler<GetUserMoneyBag, UserMoneyBagDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMoneyBagRepository _moneyBagRepository;

    public GetUserMoneyBagHandler(IUserRepository userRepository, IMoneyBagRepository moneyBagRepository)
    {
        _userRepository = userRepository;
        _moneyBagRepository = moneyBagRepository;
    }

    public async Task<UserMoneyBagDto> QueryAsync(GetUserMoneyBag query)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        if (user is null)
        {
            // ??
        }

        var moneyBag = await _moneyBagRepository.GetMoneyBagByUserId(query.UserId);
        if (moneyBag is null)
        {
            throw new MoneyBagNotFoundException(query.UserId);
        }

        return new UserMoneyBagDto
        {
            UserId = moneyBag.UserId,
            Username = user.Username,
            Gold = moneyBag.Gold,
        };
    }
}