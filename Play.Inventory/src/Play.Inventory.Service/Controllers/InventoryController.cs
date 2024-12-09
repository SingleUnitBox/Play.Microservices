﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Common.Context;
using Play.Common.Controllers;
using Play.Inventory.Application;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Service.Controllers;

public class InventoryController : BaseController
{
    private readonly IContext _context;

    public InventoryController(IContext context)
    {
        _context = context;
    }

    // [HttpGet]
    // [Authorize]
    // public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync()
    // {
    //     var userId = _context.IdentityContext.UserId;
    //     if (userId == Guid.Empty)
    //     {
    //         return BadRequest();
    //     }
    //
    //     var catalogItems = await _catalogItemsRepository.GetAllAsync();
    //     var inventoryItems = await _itemsRepository.GetAllAsync(i => i.UserId == userId);
    //
    //     var inventoryItemsDtos = inventoryItems.Select(i =>
    //     {
    //         var catalogItem = catalogItems.SingleOrDefault(c => c.Id == i.CatalogItemId);
    //         
    //         return i.AsDto(catalogItem.Name, catalogItem.Description);
    //     });
    //
    //     return Ok(inventoryItemsDtos);
    // }
    //
    // [HttpPost]
    // public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    // {
    //     var inventoryItem = await _itemsRepository.GetAsync(item => item.UserId == grantItemsDto.UserId
    //         && item.CatalogItemId == grantItemsDto.CatalogItemId);
    //     if (inventoryItem == null)
    //     {
    //         inventoryItem = new InventoryItem
    //         {
    //             CatalogItemId = grantItemsDto.CatalogItemId,
    //             UserId = grantItemsDto.UserId,
    //             Quantity = grantItemsDto.Quantity,
    //             AcquiredDate = DateTimeOffset.UtcNow
    //         };
    //         
    //         await _itemsRepository.CreateAsync(inventoryItem);
    //     }
    //     else
    //     {
    //         inventoryItem.Quantity += grantItemsDto.Quantity;
    //         await _itemsRepository.UpdateAsync(inventoryItem);
    //     }
    //
    //     return NoContent();
    // }
}