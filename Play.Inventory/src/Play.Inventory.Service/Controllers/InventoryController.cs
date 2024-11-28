using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class InventoryController : ControllerBase
{
    private readonly IRepository<InventoryItem> _itemsRepository;
    private readonly CatalogClient _catalogClient;

    public InventoryController(IRepository<InventoryItem> itemsRepository,
        CatalogClient catalogClient)
    {
        _itemsRepository = itemsRepository;
        _catalogClient = catalogClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }

        var catalogItems = await _catalogClient.GetCatalogItemsAsync();
        var inventoryItems = await _itemsRepository.GetAllAsync(i => i.UserId == userId);

        var inventoryItemsDtos = inventoryItems.Select(i =>
        {
            var catalogItem = catalogItems.SingleOrDefault(c => c.Id == i.CatalogItemId);
            
            return i.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(inventoryItemsDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var inventoryItem = await _itemsRepository.GetAsync(item => item.UserId == grantItemsDto.UserId
            && item.CatalogItemId == grantItemsDto.CatalogItemId);
        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = grantItemsDto.CatalogItemId,
                UserId = grantItemsDto.UserId,
                Quantity = grantItemsDto.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow
            };
            
            await _itemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDto.Quantity;
            await _itemsRepository.UpdateAsync(inventoryItem);
        }

        return NoContent();
    }
}