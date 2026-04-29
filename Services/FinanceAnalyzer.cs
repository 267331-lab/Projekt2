using NGOFinanceDashboard.Models;

namespace NGOFinanceDashboard.Services;

/// <summary>
/// Performs financial calculations and analysis on transaction data
/// </summary>
public class FinanceAnalyzer //: IDataAnalyzer
{
    public FinanceReport Analyze(IEnumerable<Transaction> transactions)
    {
        var transactionList = transactions.ToList();
        
        var report = new FinanceReport
        {
            TotalIncome = transactionList.Where(t => t.Amount > 0).Sum(t => t.Amount),
            TotalExpenses = transactionList.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount)),
            TransactionCount = transactionList.Count,
            AnalysisDate = DateTime.Now
        };

        report.NetBalance = report.TotalIncome - report.TotalExpenses;
        
        return report;
    }
}