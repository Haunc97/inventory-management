using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task AddAsync(Product product)
        {
            using var db = contextFactory.CreateDbContext();

            // EF Core doesn't have a tracking for the inventories
            // -> so it will be consider each one of those inventories under this particular product as new inventories
            // and tries to insert those inventories into the database, so that is the problem.
            FlagInventoryUnchanged(product, db);

            db.Products.Add(product);

            await db.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            using var db = contextFactory.CreateDbContext();
            return await db.Products.Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            using var db = contextFactory.CreateDbContext();
            return await db.Products.Where(
                x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            using var db = contextFactory.CreateDbContext();
            var prod = await db.Products
                                .Include(x => x.ProductInventories)
                                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Quantity = product.Quantity;
                prod.Price = product.Price;
                prod.ProductInventories = product.ProductInventories;

                // EF Core doesn't have a tracking for the inventories
                // -> so it will be consider each one of those inventories under this particular product as new inventories
                // and tries to insert those inventories into the database, so that is the problem.
                FlagInventoryUnchanged(prod, db);

                await db.SaveChangesAsync();
            }
        }

        private void FlagInventoryUnchanged(Product product, DbContext db)
        {
            if (product?.ProductInventories != null &&
                product.ProductInventories.Any())
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    if (prodInv.Inventory != null) db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                }
            }
        }
    }
}