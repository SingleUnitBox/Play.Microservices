using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;
using Play.Inventory.Application.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Policies;
using Play.Inventory.Domain.Policies.Factories;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Commands.Handlers;

public class PurchaseItemHandler : ICommandHandler<PurchaseItem>
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IMoneyBagRepository _moneyBagRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IWeaponPurchasePolicyFactory _policyFactory;
    private readonly IEventDispatcher _eventDispatcher;

    public PurchaseItemHandler(ICatalogItemRepository catalogItemRepository,
        IInventoryItemRepository inventoryItemRepository, 
        IMoneyBagRepository moneyBagRepository, 
        IPlayerRepository playerRepository,
        IWeaponPurchasePolicyFactory policyFactory,
        IEventDispatcher eventDispatcher)
    {
        _catalogItemRepository = catalogItemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _moneyBagRepository = moneyBagRepository;
        _playerRepository = playerRepository;
        _policyFactory = policyFactory;
        _eventDispatcher = eventDispatcher;
    }
    
    public async Task HandleAsync(PurchaseItem command)
    {
        var catalogItem = await ValidateCatalogItem(command.ItemId);
        var player = await ValidatePlayer(command.PlayerId);
        var moneyBag = await ValidateMoneyBag(command.PlayerId);
        var inventoryItem = await _inventoryItemRepository.GetInventory(i =>
            i.PlayerId == player.Id && i.CatalogItemId == command.ItemId);
        
        var purchasePolicy = _policyFactory.Create(moneyBag);

        if (inventoryItem is null)
        {
            inventoryItem = InventoryItem
                .Create(catalogItem.Id, player.Id, command.Quantity, DateTimeOffset.UtcNow);
            CanCatalogItemBePurchased(purchasePolicy, catalogItem, inventoryItem);
            IsEnoughGoldToPurchase(moneyBag.Gold, catalogItem, command.Quantity);
            
            await _inventoryItemRepository.CreateAsync(inventoryItem);
        }
        else
        {
            CanCatalogItemBePurchased(purchasePolicy, catalogItem, inventoryItem);
            IsEnoughGoldToPurchase(moneyBag.Gold, catalogItem, command.Quantity);
            
            inventoryItem.AddQuantity(command.Quantity);
            await _inventoryItemRepository.UpdateAsync(inventoryItem);
        }
        
        // 1st approach
        // second update in handler, needs to be atomic with inventoryItem creation/update
        // or event ItemPurchased shall be generated and send by eventDispatcher
        // moneyBag.SubtractGold(command.Quantity * catalogItem.Price);
        // await _moneyBagRepository.UpdateMoneyBag(moneyBag);
        
        // 2nd approach
        await _eventDispatcher.HandleAsync(new ItemPurchased(player.Id, catalogItem.Id, command.Quantity));
    }
    
    private void CanCatalogItemBePurchased(IWeaponPurchasePolicy policy, CatalogItem catalogItem, InventoryItem inventoryItem)
    {
        var canBePurchased = policy.CanCatalogItemBePurchased(catalogItem, inventoryItem);
        if (canBePurchased is false)
        {
            throw new CatalogItemCannotBePurchasedException(catalogItem.Id, catalogItem.Name);
        }
    }

    private void IsEnoughGoldToPurchase(decimal availableGold, CatalogItem catalogItem, int quantity)
    {
        var totalGoldRequired = quantity * catalogItem.Price;
        if (availableGold < totalGoldRequired)
        {
            throw new NotEnoughGoldToPurchaseException(totalGoldRequired, quantity, catalogItem.Name);
        }
    }

    private async Task<CatalogItem> ValidateCatalogItem(Guid itemId)
    {
        var catalogItem = await _catalogItemRepository.GetByIdAsync(itemId);
        if (catalogItem is null)
        {
            throw new CatalogItemNotFoundException(itemId);
        }
        
        return catalogItem;
    }
    
    private async Task<Player> ValidatePlayer(Guid userId)
    {
        var user = await _playerRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new PlayerNotFoundException(userId);
        }
        
        return user;
    }

    private async Task<MoneyBag> ValidateMoneyBag(Guid userId)
    {
        var moneyBag = await _moneyBagRepository.GetMoneyBagByPlayerId(userId);
        if (moneyBag is null)
        {
            throw new MoneyBagNotFoundException(userId);
        }
        
        return moneyBag;
    }
}