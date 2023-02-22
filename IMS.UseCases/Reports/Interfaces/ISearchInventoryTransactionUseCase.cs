using IMS.CoreBusiness;

namespace IMS.UseCases.Reports.Interfaces
{
    public interface ISearchInventoryTransactionUseCase
    {
        Task<IEnumerable<InventoryTransation>> ExecuteAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransationType? transactionType);
    }
}