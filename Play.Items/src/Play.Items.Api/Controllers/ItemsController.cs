using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.Items.Application.Commands;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;

namespace Play.Items.Api.Controllers
{
    [ApiVersion("1.0")]
    public class ItemsController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ItemsController(ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = await _queryDispatcher.QueryAsync(new GetItems());
            return Ok(items);
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid itemId)
            => OkOrNotFound(await _queryDispatcher.QueryAsync(new GetItem(itemId)));

        [HttpPost("crafter")]
        public async Task<ActionResult> CreateItemWithCrafter(CreateItemWithCrafter command)
        {
            await _commandDispatcher.DispatchAsync(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { itemId = command.ItemId }, null);
        }

        [HttpPost]
        public async Task<ActionResult> CreateItemAsync(CreateItem command)
        {
            await _commandDispatcher.DispatchAsync(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { itemId = command.ItemId }, null);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateAsync(Guid itemId, UpdateItem command)
        {
            await _commandDispatcher.DispatchAsync(command with { ItemId = itemId });
            return NoContent();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteAsync(Guid itemId)
        {
            await _commandDispatcher.DispatchAsync(new DeleteItem(itemId));
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllAsync()
        {
            await _commandDispatcher.DispatchAsync(new DeleteItems());
            return NoContent();
        }
    }
}