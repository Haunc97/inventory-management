namespace IMS.UseCases.Activities;

public class SellProductUseCase(IProductTransactionRepository productTransationRepository, IProductRepository productRepository) : ISellProductUseCase
{
    public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        //store transaction record
        await productTransationRepository.SellAsync(salesOrderNumber, product, quantity, unitPrice, doneBy);

        //decrease product quantity
        product.Quantity -= quantity;
        await productRepository.UpdateAsync(product);
    }
}
