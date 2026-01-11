namespace IMS.UseCases.Inventories;

public class AddInventoryUseCase(IInventoryRepository inventoryRepository) : IAddInventoryUseCase
{
    public async Task ExecuteAsync(Inventory inventory)
    {
        await inventoryRepository.AddAsync(inventory);
    }
}
