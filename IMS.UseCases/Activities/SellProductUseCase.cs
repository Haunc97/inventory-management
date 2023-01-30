using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class SellProductUseCase : ISellProductUseCase
    {
        private readonly IProductTransationRepository productTransationRepository;
        private readonly IProductRepository productRepository;

        public SellProductUseCase(IProductTransationRepository productTransationRepository, IProductRepository productRepository)
        {
            this.productTransationRepository = productTransationRepository;
            this.productRepository = productRepository;
        }

        public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            //store transaction record
            await productTransationRepository.SellAsync(salesOrderNumber, product, quantity, unitPrice, doneBy);

            //decrease product quantity
            product.Quantity -= quantity;
            await productRepository.UpdateAsync(product);
        }
    }
}
