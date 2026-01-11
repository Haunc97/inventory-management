namespace IMS.UseCases.Products;

public class ViewProductByIdUseCase(IProductRepository productRepository) : IViewProductByIdUseCase
{
    public async Task<Product?> ExecuteAsync(int productId)
    {
        return await productRepository.GetByIdAsync(productId);
    }
}