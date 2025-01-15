using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Mongo.Queries.Handlers;

public class GetUserMoneyBagHandler //: IQueryHandler<GetUserMoneyBag, UserMoneyBagDto>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMoneyBagRepository _moneyBagRepository;

    public GetUserMoneyBagHandler(IPlayerRepository playerRepository, IMoneyBagRepository moneyBagRepository)
    {
        _playerRepository = playerRepository;
        _moneyBagRepository = moneyBagRepository;
    }

    public async Task<UserMoneyBagDto> QueryAsync(GetUserMoneyBag query)
    {
        var player = await _playerRepository.GetByIdAsync(query.PlayerId);
        if (player is null)
        {
            // ??
        }

        var moneyBag = await _moneyBagRepository.GetMoneyBagByPlayerId(query.PlayerId);
        if (moneyBag is null)
        {
            throw new MoneyBagNotFoundException(query.PlayerId);
        }

        return new UserMoneyBagDto
        {
            PlayerId = moneyBag.PlayerId,
            Username = player.Username,
            Gold = moneyBag.Gold,
        };
    }
}