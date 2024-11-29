using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> repository,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await _repository.GetAllAsync())
                .Select(i => i.AsDto());
            
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            };

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await _repository.CreateAsync(item);
            await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemCreated(
                item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            };

            item.Name = updateItemDto.Name;
            item.Description = updateItemDto.Description;
            item.Price = updateItemDto.Price;
            
            await _repository.UpdateAsync(item);
            await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemUpdated(
                item.Id, item.Name, item.Description));
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _repository.RemoveAsync(id);
            await _publishEndpoint.Publish(new Contracts.Contracts.CatalogItemDeleted(id));
            
            return NoContent();
        }
    }
}