namespace IMS.UseCases.Activities;

public class ProduceProductUseCase(IProductTransactionRepository productTransationRepository, IProductRepository productRepository) : IProduceProductUseCase
{
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
