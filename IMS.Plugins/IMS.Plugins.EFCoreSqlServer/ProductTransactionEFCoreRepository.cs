using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionEFCoreRepository : IProductTransactionRepository
    {
        private readonly IProductRepository productRepository;
        private readonly IInventoryTransationRepository inventoryTransationRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductTransactionEFCoreRepository(IProductRepository productRepository,
            IInventoryTransationRepository inventoryTransationRepository,
            IInventoryRepository inventoryRepository, IDbContextFactory<IMSContext> contextFactory)
        {
            this.productRepository = productRepository;
            this.inventoryTransationRepository = inventoryTransationRepository;
            this.inventoryRepository = inventoryRepository;
            this.contextFactory = contextFactory;
        }
        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            using var db = contextFactory.CreateDbContext();

            var prod = await productRepository.GetByIdAsync(product.ProductId);
            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    if (pi.Inventory != null)
                    {
                        //add inventory transation
                        await inventoryTransationRepository.ProduceAsync(
                            productionNumber,
                            pi.Inventory,
                            pi.InventoryQuantity * quantity,
                            doneBy,
                            -1);

                        //decrease the inventories
                        var inv = await inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;
                        await inventoryRepository.UpdateAsync(inv);
                    }
                }
            }

            //add product transation
            db.ProductTransations.Add(new ProductTransation
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransationType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransationDate = DateTime.Now,
                DoneBy = doneBy
            });

            await db.SaveChangesAsync();
        }

        public async Task SellAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            using var db = contextFactory.CreateDbContext();

            db.ProductTransations.Add(new ProductTransation
            {
                SONumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransationType.SellProduct,
                QuantityAfter = product.Quantity - quantity,
                TransationDate = DateTime.Now,
                UnitPrice = unitPrice,
                DoneBy = doneBy
            });

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductTransation>> GetProductTransactionAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransationType? activityType)
        {
            using var db = contextFactory.CreateDbContext();

            var query = from p in db.Products
                        join pt in db.ProductTransations on p.ProductId equals pt.ProductId
                        where
                            (string.IsNullOrWhiteSpace(productName) || p.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                            &&
                            (!dateFrom.HasValue || pt.TransationDate >= dateFrom.Value)
                            &&
                            (!dateTo.HasValue || pt.TransationDate <= dateTo.Value)
                            &&
                            (!activityType.HasValue || pt.ActivityType == activityType.Value)
                        select pt;

            return await query.Include(x => x.Product).ToListAsync();
        }
    }
}