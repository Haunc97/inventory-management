namespace IMS.UseCases.Inventories;

public class EditInventoryUseCase(IInventoryRepository inventoryRepository) : IEditInventoryUseCase
{
    public async Task ExecuteAsync(Inventory inventory)
    {
        await inventoryRepository.UpdateAsync(inventory);
    }
}