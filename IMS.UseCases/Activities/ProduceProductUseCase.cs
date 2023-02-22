using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductTransactionRepository productTransationRepository;
        private readonly IProductRepository productRepository;

        public ProduceProductUseCase(IProductTransactionRepository productTransationRepository, IProductRepository productRepository)
        {
            this.productTransationRepository = productTransationRepository;
            this.productRepository = productRepository;
        }

        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //add transation record
            await productTransationRepository.ProduceAsync(productionNumber, product, quantity, doneBy);

            //decrease the quantity inventories

            //update the quantity of products
            product.Quantity += quantity;
            await productRepository.UpdateAsync(product);
        }
    }
}
