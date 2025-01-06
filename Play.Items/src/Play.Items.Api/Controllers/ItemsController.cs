using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.Items.Application.Commands;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;
using Play.Items.Domain.Repositories;

namespace Play.Items.Api.Controllers
{
    public class ItemsController : BaseController
    {
        private readonly IItemRepository _itemRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IBusPublisher _busPublisher;

        public ItemsController(IItemRepository itemRepository,
            IPublishEndpoint publishEndpoint,
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            _publishEndpoint = publishEndpoint;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _itemRepository = itemRepository;
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
    
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItem command)
        {
            await _commandDispatcher.DispatchAsync(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { itemId = command.ItemId }, null);
        }
        
        // [HttpPost]
        // public async Task<ActionResult> CreateItemAsync(CreateItem command)
        // {
        //     await _publishEndpoint.Publish(command);
        //     return CreatedAtAction(nameof(GetByIdAsync), new { itemId = command.Id }, null);
        // }
        
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateAsync(Guid itemId, UpdateItem command)
        {
            await _commandDispatcher.DispatchAsync(command with { ItemId = itemId });
            return NoContent();
        }
        
        // [HttpPut("{itemId}")]
        // public async Task<IActionResult> UpdateAsync(Guid itemId, UpdateItem command)
        // {
        //     await _publishEndpoint.Publish(command with { ItemId = itemId });
        //     return NoContent();
        // }
        
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteAsync(Guid itemId)
        {
            await _commandDispatcher.DispatchAsync(new DeleteItem(itemId));
            return NoContent();
        }
        
        // [HttpDelete("{itemId}")]
        // public async Task<IActionResult> DeleteAsync(Guid itemId)
        // {
        //     await _publishEndpoint.Publish(new DeleteItem(itemId));
        //     return NoContent();
        // }
    }
}