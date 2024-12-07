using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Application.Commands;
using Play.Catalog.Application.DTO;
using Play.Catalog.Domain.Repositories;
using Play.Common.Temp.Commands;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICommandDispatcher _commandDispatcher;

        public ItemsController(IItemRepository itemRepository,
            IPublishEndpoint publishEndpoint,
            ICommandDispatcher commandDispatcher)
        {
            _publishEndpoint = publishEndpoint;
            _commandDispatcher = commandDispatcher;
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await _itemRepository.GetAllAsync())
                .Select(i => i.AsDto());
            
            return Ok(items);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            };
        
            return item.AsDto();
        }
        
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItem command)
        {
            await _commandDispatcher.DispatchAsync(command);
            
            // await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemCreated(
            //     command.Id, command.Name, command.Description));
            //
            // return CreatedAtAction(nameof(GetByIdAsync), new { id = command.Id }, null);

            return Ok();
        }
        
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateAsync(Guid itemId, UpdateItem command)
        {
            await _commandDispatcher.DispatchAsync(command with { ItemId = itemId });
            // await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemUpdated(
            //     item.Id, item.Name, item.Description));
            
            return NoContent();
        }
        
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteAsync(Guid itemId)
        {
            await _commandDispatcher.DispatchAsync(new DeleteItem(itemId));
            //await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemDeleted(id));
            
            return NoContent();
        }
    }
}