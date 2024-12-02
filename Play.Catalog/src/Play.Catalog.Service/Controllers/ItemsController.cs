using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAggregateItemRepository _aggregateItemRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IItemRepository itemRepository,
            IPublishEndpoint publishEndpoint,
            IAggregateItemRepository aggregateItemRepository)
        {
            _publishEndpoint = publishEndpoint;
            _aggregateItemRepository = aggregateItemRepository;
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
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            var item = new Item(createItemDto.Name, createItemDto.Description, createItemDto.Price);
        
            await _itemRepository.CreateAsync(item);
            await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemCreated(
                item.Id, item.Name, item.Description));
        
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, null);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            };
        
            item.Name = updateItemDto.Name;
            item.Description = updateItemDto.Description;
            item.Price = updateItemDto.Price;
            
            await _itemRepository.UpdateAsync(item);
            await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemUpdated(
                item.Id, item.Name, item.Description));
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _itemRepository.DeleteAsync(id);
            await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemDeleted(id));
            
            return NoContent();
        }

        [HttpPost("aggregate")]
        public async Task<ActionResult> PostAggregate(CreateAggregateItemDto dto)
        {
            var aggregateItem = new AggregateItem(dto.Name, dto.Description, dto.Price);
            await _aggregateItemRepository.CreateAsync(aggregateItem);
            return NoContent();
        }
    }
}