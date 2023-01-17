using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product { ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 50 },
                new Product { ProductId = 2, ProductName = "Car", Quantity = 5, Price = 25000 }
            };
        }

        public Task AddAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var maxId = _products.Max(x => x.ProductId);
            product.ProductId = maxId + 1;

            _products.Add(product);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);

            return await Task.FromResult(_products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList());
        }


    }
}