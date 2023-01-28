using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransationRepository inventoryTransationRepository;
        private readonly IInventoryRepository inventoryRepository;
        public PurchaseInventoryUseCase(IInventoryTransationRepository inventoryTransationRepository, IInventoryRepository inventoryRepository)
        {
            this.inventoryTransationRepository = inventoryTransationRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
        {
            // insert a record in the transation table
            inventoryTransationRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

            // increase the quantity
            inventory.Quantity += quantity;
            await inventoryRepository.UpdateAsync(inventory);
        }
    }
}