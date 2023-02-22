using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;

namespace IMS.UseCases.Reports
{
    public class SearchProductTransactionUseCase : ISearchProductTransactionUseCase
    {
        private readonly IProductTransactionRepository productTransactionRepository;

        public SearchProductTransactionUseCase(IProductTransactionRepository productTransactionRepository)
        {
            this.productTransactionRepository = productTransactionRepository;
        }
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
}