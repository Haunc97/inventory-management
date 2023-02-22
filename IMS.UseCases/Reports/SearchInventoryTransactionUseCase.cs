using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;

namespace IMS.UseCases.Reports
{
    public class SearchInventoryTransactionUseCase : ISearchInventoryTransactionUseCase
    {
        private readonly IInventoryTransationRepository inventoryTransationRepository;

        public SearchInventoryTransactionUseCase(IInventoryTransationRepository inventoryTransationRepository)
        {
            this.inventoryTransationRepository = inventoryTransationRepository;
        }
        public async Task<IEnumerable<InventoryTransation>> ExecuteAsync(
            string inventoryName,
            DateTime? dateFrom,
            DateTime? dateTo,
            InventoryTransationType? transactionType)
        {
            if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

            return await inventoryTransationRepository.GetInventoryTransationsAsync(
                inventoryName,
                dateFrom,
                dateTo,
                transactionType
                );
        }
    }
}
