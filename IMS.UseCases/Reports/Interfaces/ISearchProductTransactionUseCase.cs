using IMS.CoreBusiness;

namespace IMS.UseCases.Reports.Interfaces
{
    public interface ISearchProductTransactionUseCase
    {
        Task<IEnumerable<ProductTransation>> ExecuteAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransationType? activityType);
    }
}