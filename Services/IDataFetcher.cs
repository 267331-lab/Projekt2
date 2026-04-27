using NGOFinanceDashboard.Models;

namespace NGOFinanceDashboard.Services;

/// <summary>
/// Interface for data fetching operations
/// </summary>
public interface IDataFetcher
{
    /// <summary>
    /// Fetches transactions from the data source
    /// </summary>
    Task<IEnumerable<Transaction>> FetchTransactionsAsync(CancellationToken cancellationToken = default);
}