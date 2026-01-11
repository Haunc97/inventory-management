using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using IMS.Plugins.EFCoreSqlServer;
using IMS.Plugins.InMemory;
using IMS.UseCases.Inventories;
using IMS.UseCases.Products;
using IMS.UseCases.Activities;
using IMS.UseCases.Reports;

namespace IMS.WebApp.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var constr = configuration.GetConnectionString("InventoryManagement");

        // EF Core for Identity
        services.AddDbContext<AccountDbContext>(options =>
        {
            options.UseSqlServer(constr);
        });

        // Identity
        services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
        }).AddEntityFrameworkStores<AccountDbContext>();

        // IMS DbContext
        services.AddDbContextFactory<IMSContext>(options =>
        {
            options.UseSqlServer(constr);
        });

        // Repositories
        if (environment.IsEnvironment("TESTING"))
        {
            services.AddSingleton<IInventoryRepository, InventoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IInventoryTransationRepository, InventoryTransationRepository>();
            services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
        }
        else
        {
            services.AddTransient<IInventoryRepository, InventoryEFCoreRepository>();
            services.AddTransient<IProductRepository, ProductEFCoreRepository>();
            services.AddTransient<IInventoryTransationRepository, InventoryTransationEFCoreRepository>();
            services.AddTransient<IProductTransactionRepository, ProductTransactionEFCoreRepository>();
        }

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
        services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
        services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
        services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();

        services.AddTransient<IViewProductsByNameUseCase, ViewProductsByNameUseCase>();
        services.AddTransient<IAddProductUseCase, AddProductUseCase>();
        services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();
        services.AddTransient<IEditProductUseCase, EditProductUseCase>();

        services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();
        services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();
        services.AddTransient<ISellProductUseCase, SellProductUseCase>();

        services.AddTransient<ISearchInventoryTransactionUseCase, SearchInventoryTransactionUseCase>();
        services.AddTransient<ISearchProductTransactionUseCase, SearchProductTransactionUseCase>();

        return services;
    }
}
