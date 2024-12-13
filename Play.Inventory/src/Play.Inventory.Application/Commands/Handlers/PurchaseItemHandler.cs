using Play.Common.Abs.Commands;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Commands.Handlers;

public class PurchaseItemHandler : ICommandHandler<PurchaseItem>
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IMoneyBagRepository _moneyBagRepository;
    private readonly IUserRepository _userRepository;
    
    public PurchaseItemHandler(ICatalogItemRepository catalogItemRepository,
        IInventoryItemRepository inventoryItemRepository, 
        IMoneyBagRepository moneyBagRepository, 
        IUserRepository userRepository)
    {
        _catalogItemRepository = catalogItemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _moneyBagRepository = moneyBagRepository;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(PurchaseItem command)
    {
        var catalogItem = await _catalogItemRepository.GetByIdAsync(command.ItemId);
        if (catalogItem is null)
        {
            throw new CatalogItemNotFoundException(command.ItemId);
        }
        
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }

        var moneyBag = await _moneyBagRepository.GetMoneyBagByUserId(user.Id);
        if (moneyBag is null)
        {
            throw new MoneyBagNotFoundException(command.UserId);
        }
        
        var totalGold = TotalGoldRequiredForPurchasing(command.Quantity, catalogItem.Price);
        if (moneyBag.Gold < totalGold)
        {
            throw new NotEnoughGoldToPurchaseException(totalGold, command.Quantity, catalogItem.Name);
        }
        
        moneyBag.SubtractGold(totalGold);
        await _moneyBagRepository.UpdateMoneyBag(moneyBag);

        var inventoryItem = await _inventoryItemRepository.GetInventory(i =>
            i.UserId == user.Id && i.CatalogItemId == command.ItemId);
        if (inventoryItem is not null)
        {
            inventoryItem.AddQuantity(command.Quantity);
            await _inventoryItemRepository.UpdateAsync(inventoryItem);
        }
        else
        {
            inventoryItem = InventoryItem
                .Create(catalogItem.Id, user.Id, command.Quantity, DateTimeOffset.UtcNow);
            await _inventoryItemRepository.CreateAsync(inventoryItem);
        }
    }
    
    private decimal TotalGoldRequiredForPurchasing(int quantity, decimal itemPrice)
        => quantity * itemPrice;
}