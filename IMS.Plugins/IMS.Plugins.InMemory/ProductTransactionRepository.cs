using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private List<ProductTransation> _productTransations = new List<ProductTransation>();

        private readonly IProductRepository productRepository;
        private readonly IInventoryTransationRepository inventoryTransationRepository;
        private readonly IInventoryRepository inventoryRepository;

        public ProductTransactionRepository(IProductRepository productRepository,
            IInventoryTransationRepository inventoryTransationRepository,
            IInventoryRepository inventoryRepository)
        {
            this.productRepository = productRepository;
            this.inventoryTransationRepository = inventoryTransationRepository;
            this.inventoryRepository = inventoryRepository;
        }
        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            var prod = await productRepository.GetByIdAsync(product.ProductId);
            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    if (pi.Inventory != null)
                    {
                        //add inventory transation
                        inventoryTransationRepository.ProduceAsync(
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
            _productTransations.Add(new ProductTransation
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransationType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransationDate = DateTime.Now,
                DoneBy = doneBy
            });
        }

        public Task SellAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            _productTransations.Add(new ProductTransation
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

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransation>> GetProductTransactionAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransationType? activityType)
        {
            var prods = (await productRepository.GetByNameAsync(string.Empty)).ToList();

            var query = from p in prods
                    join pt in _productTransations on p.ProductId equals pt.ProductId
                    where
                        (string.IsNullOrWhiteSpace(productName) || p.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || pt.TransationDate >= dateFrom.Value)
                        &&
                        (!dateTo.HasValue || pt.TransationDate <= dateTo.Value)
                        &&
                        (!activityType.HasValue || pt.ActivityType == activityType.Value)
                    select new ProductTransation
                    {
                        Product = p,
                        ProductId = pt.ProductId,
                        ProductTransationId = pt.ProductTransationId,
                        ProductionNumber = pt.ProductionNumber,
                        SONumber = pt.SONumber,
                        ActivityType = pt.ActivityType,
                        QuantityAfter = pt.QuantityAfter,
                        QuantityBefore = pt.QuantityBefore,
                        TransationDate = pt.TransationDate,
                        UnitPrice = pt.UnitPrice,
                        DoneBy = pt.DoneBy
                    };
            return query;
        }
    }
}