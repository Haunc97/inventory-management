namespace IMS.UseCases.Inventories;

public class ViewInventoryByIdUseCase(IInventoryRepository inventoryRepository) : IViewInventoryByIdUseCase
{
    public async Task<Inventory> ExecuteAsync(int inventoryId)
    {
        return await inventoryRepository.GetInventoryByIdAsync(inventoryId);
    }
}