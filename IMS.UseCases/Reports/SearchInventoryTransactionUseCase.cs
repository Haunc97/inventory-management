namespace IMS.UseCases.Reports;

public class SearchInventoryTransactionUseCase(IInventoryTransationRepository inventoryTransationRepository) : ISearchInventoryTransactionUseCase
{
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
