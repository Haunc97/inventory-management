using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class InventoryTransationEFCoreRepository : IInventoryTransationRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public InventoryTransationEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<IEnumerable<InventoryTransation>> GetInventoryTransationsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransationType? transactionType)
        {
            using var db = contextFactory.CreateDbContext();

            var query = from it in db.InventoryTransations
                        join inv in db.Inventories on it.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                            &&
                            (!dateFrom.HasValue || it.TransationDate >= dateFrom.Value.Date)
                            &&
                            (!dateTo.HasValue || it.TransationDate <= dateTo.Value.Date)
                            &&
                            (!transactionType.HasValue || it.ActivityType == transactionType)
                        select it;

            return await query.Include(x => x.Inventory).ToListAsync();
        }

        public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            using var db = contextFactory.CreateDbContext();

            db.InventoryTransations.Add(new InventoryTransation
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

            await db.SaveChangesAsync();
        }

        public async Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            using var db = contextFactory.CreateDbContext();

            db.InventoryTransations.Add(new InventoryTransation
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
            await db.SaveChangesAsync();
        }
    }
}