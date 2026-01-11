namespace IMS.UseCases.Activities;

public class PurchaseInventoryUseCase(IInventoryTransationRepository inventoryTransationRepository, IInventoryRepository inventoryRepository) : IPurchaseInventoryUseCase
{
    public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
    {
        // insert a record in the transation table
        inventoryTransationRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

        // increase the quantity
        inventory.Quantity += quantity;
        await inventoryRepository.UpdateAsync(inventory);
    }
}