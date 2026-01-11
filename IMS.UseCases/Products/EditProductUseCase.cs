namespace IMS.UseCases.Products;

public class EditProductUseCase(IProductRepository productRepository) : IEditProductUseCase
{
    public async Task ExecuteAsync(Product product)
    {
        await productRepository.UpdateAsync(product);
    }
}