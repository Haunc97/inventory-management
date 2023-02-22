using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductTransactionRepository
    {
        Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy);
        Task SellAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy);
        Task<IEnumerable<ProductTransation>> GetProductTransactionAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransationType? activityType);
    }
}