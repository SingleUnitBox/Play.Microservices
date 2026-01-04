using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.Items.Application.Commands;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;

namespace Play.Items.Api.Controllers;

public class CraftersController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CrafterDto>>> GetCrafters()
        => Ok(await queryDispatcher.QueryAsync(new GetCrafters()));
    
    [HttpGet("{crafterId:guid}")]
    public async Task<ActionResult<CrafterDto>> GetCrafter(Guid crafterId)
        => OkOrNotFound(await queryDispatcher.QueryAsync(new GetCrafter(crafterId)));

    [HttpPost]
    public async Task<ActionResult> PostCrafter(CreateCrafter command)
    {
        await commandDispatcher.DispatchAsync(command);
        return CreatedAtAction(nameof(GetCrafter), new { crafterId = command.CrafterId }, null);
    }
}