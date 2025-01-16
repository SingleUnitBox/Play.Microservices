using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Queries;
using Play.Common.Context;
using Play.Common.Controllers;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;

namespace Play.Inventory.Service.Controllers;

public class MoneyBagController(IQueryDispatcher queryDispatcher, IContext context) : BaseController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PlayerMoneyBagDto>> GetUserMoneyBag()
        => OkOrNotFound(await queryDispatcher.QueryAsync(new GetPlayerMoneyBag(context.IdentityContext.UserId)));
}