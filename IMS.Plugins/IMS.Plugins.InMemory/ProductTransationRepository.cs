using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductTransationRepository : IProductTransationRepository
    {
        private List<ProductTransation> _productTransations = new List<ProductTransation>();

        private readonly IProductRepository productRepository;
        private readonly IInventoryTransationRepository inventoryTransationRepository;
        private readonly IInventoryRepository inventoryRepository;

        public ProductTransationRepository(IProductRepository productRepository,
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
                QuantityBefore = quantity,
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
                ProductionNumber = salesOrderNumber,
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
    }
}