using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByNameAsync(string name);
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(int productId);
        Task UpdateAsync(Product product);
    }
}