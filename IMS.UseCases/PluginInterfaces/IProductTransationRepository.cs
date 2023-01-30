using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductTransationRepository
    {
        Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy);
        Task SellAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy);
    }
}