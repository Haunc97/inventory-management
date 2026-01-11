namespace IMS.UseCases.Reports;

public class SearchProductTransactionUseCase(IProductTransactionRepository productTransactionRepository) : ISearchProductTransactionUseCase
{
    public async Task<IEnumerable<ProductTransation>> ExecuteAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransationType? activityType)
    {
        if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

        return await productTransactionRepository.GetProductTransactionAsync(
            productName,
            dateFrom,
            dateTo,
            activityType);
    }
}