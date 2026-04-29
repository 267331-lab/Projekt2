namespace NGOFinanceDashboard.Services;

using NGOFinanceDashboard.Models;

public interface IDataFetcher
{
    Task<IEnumerable<Transaction>> FetchTransactionsAsync(string fioAccountUrl, CancellationToken cancellationToken = default);
}