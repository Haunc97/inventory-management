namespace IMS.UseCases.Products;

public class ViewProductsByNameUseCase(IProductRepository productRepository) : IViewProductsByNameUseCase
{
    public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
    {
        return await productRepository.GetByNameAsync(name);
    }
}