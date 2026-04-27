using NGOFinanceDashboard.Models;

namespace NGOFinanceDashboard.Services;

/// <summary>
/// Interface for data analysis operations
/// </summary>
public interface IDataAnalyzer
{
    /// <summary>
    /// Analyzes transactions and generates a finance report
    /// </summary>
    FinanceReport Analyze(IEnumerable<Transaction> transactions);
}