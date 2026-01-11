namespace IMS.UseCases.Products;

public class AddProductUseCase(IProductRepository productRepository) : IAddProductUseCase
{
    public async Task ExecuteAsync(Product product)
    {
        if (product == null)
            return;

        await productRepository.AddAsync(product);
    }
}
