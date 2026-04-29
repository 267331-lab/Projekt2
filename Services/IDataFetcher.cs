namespace NGOFinanceDashboard.Services;

using NGOFinanceDashboard.Models;

public interface IDataFetcher
{
    Task<List<Transaction>> FetchTransactionsAsync(string fioAccountUrl);
}