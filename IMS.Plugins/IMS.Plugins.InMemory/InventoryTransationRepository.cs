using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransationRepository : IInventoryTransationRepository
    {
        private readonly IInventoryRepository inventoryRepository;
        public List<InventoryTransation> _inventoryTransations = new List<InventoryTransation>();

        public InventoryTransationRepository(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransation>> GetInventoryTransationsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransationType? transactionType)
        {
            var inventories = (await inventoryRepository.GetByNameAsync(string.Empty)).ToList();

            var query = from it in _inventoryTransations
                        join inv in inventories on it.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                            &&
                            (!dateFrom.HasValue || it.TransationDate >= dateFrom.Value.Date)
                            &&
                            (!dateTo.HasValue || it.TransationDate <= dateTo.Value.Date)
                            &&
                            (!transactionType.HasValue || it.ActivityType == transactionType)
                        select new InventoryTransation
                        {
                            Inventory = inv,
                            InventoryTransationId = it.InventoryTransationId,
                            PONumber = it.PONumber,
                            ProductionNumber = it.ProductionNumber,
                            InventoryId = it.InventoryId,
                            QuantityBefore = it.QuantityBefore,
                            ActivityType = it.ActivityType,
                            QuantityAfter = it.QuantityAfter,
                            TransationDate = it.TransationDate,
                            DoneBy = it.DoneBy,
                            UnitPrice = it.UnitPrice
                        };
            return query;
        }

        public void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            _inventoryTransations.Add(new InventoryTransation
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransationType.ProduceProduct,
                QuantityAfter = inventory.Quantity - quantityToConsume,
                TransationDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });
        }

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