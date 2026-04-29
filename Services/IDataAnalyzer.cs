namespace NGOFinanceDashboard.Services;

using NGOFinanceDashboard.Models;

public interface IDataAnalyzer
{
    decimal CalculateCashFlow(List<Transaction> transactions);
    Transaction FindBiggestExpense(List<Transaction> transactions);
    List<(string Contributor, decimal Total)> GetTop3Contributors(List<Transaction> transactions);
    Dictionary<string, int> GetMostCommonMessages(List<Transaction> transactions);
}   