namespace NGOFinanceDashboard.Models;

/// <summary>
/// Represents the results of financial analysis
/// </summary>
public class FinanceReport
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetBalance { get; set; }
    public int TransactionCount { get; set; }
    public DateTime AnalysisDate { get; set; }
    public Dictionary<string, decimal> CategoryBreakdown { get; set; } = new();
}