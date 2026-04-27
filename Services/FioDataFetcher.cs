using NGOFinanceDashboard.Models;

namespace NGOFinanceDashboard.Services;

/// <summary>
/// Fetches transaction data from Fio Bank
/// </summary>
public class FioDataFetcher : IDataFetcher
{
    private readonly HttpClient _httpClient;

    public FioDataFetcher(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<Transaction>> FetchTransactionsAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Implement Fio Bank scraping logic
        // This will require API token configuration and proper HTTP requests
        throw new NotImplementedException("FioDataFetcher implementation pending");
    }
}