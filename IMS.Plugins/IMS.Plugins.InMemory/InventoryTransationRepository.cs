using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransationRepository : IInventoryTransationRepository
    {
        public List<InventoryTransation> _inventoryTransations = new List<InventoryTransation>();

        public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            _inventoryTransations.Add(new InventoryTransation
            {
                PONumber = poNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransationType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransationDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });
        }
    }
}