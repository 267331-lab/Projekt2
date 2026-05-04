namespace NGOFinanceDashboard.Models;

/// <summary>
/// Represents the results of financial analysis
/// </summary>
public class FinanceReport
{

    public List<(string, decimal)> TopContributors { get; set; }
    public Dictionary<string, int> Messages { get; set; }
    public decimal CashFlow { get; set; }
    public Transaction BiggestExpense { get; set; }

}