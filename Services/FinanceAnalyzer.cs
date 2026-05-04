namespace NGOFinanceDashboard.Services;

using NGOFinanceDashboard.Models;

/// <summary>
/// Analyzes financial transaction data and provides business insights.
/// Implements SOLID: Single Responsibility Principle
/// </summary>
public class FinanceAnalyzer : IDataAnalyzer
{
    /// <summary>
    /// Calculate total cash flow (sum of all transactions)
    /// Positive = net income, Negative = net expense
    /// </summary>
    public decimal CalculateCashFlow(List<Transaction> transactions)
    {
        if (transactions == null || transactions.Count == 0)
            return 0m;

        return transactions.Sum(t => t.Amount);
    }

    /// <summary>
    /// Find the single biggest expense (most negative amount)
    /// </summary>
    public Transaction? FindBiggestExpense(List<Transaction> transactions)
    {
        if (transactions == null || transactions.Count == 0)
            return null;

        // Filter for expenses only (negative amounts)
        var expenses = transactions.Where(t => t.IsExpense).ToList();

        if (expenses.Count == 0)
            return null;

        // Return transaction with smallest (most negative) amount
        return expenses.OrderBy(t => t.Amount).First();
    }

    /// <summary>
    /// Get top 3 contributors (senders with largest total amounts)
    /// </summary>
    public List<(string Contributor, decimal Total)> GetTop3Contributors(List<Transaction> transactions)
    {
        if (transactions == null || transactions.Count == 0)
            return new List<(string, decimal)>();

        // Filter income only (positive amounts)
        var income = transactions.Where(t => t.IsIncome).ToList();

        if (income.Count == 0)
            return new List<(string, decimal)>();

        // Group by sender, sum amounts, order by total descending
        var topContributors = income
            .GroupBy(t => t.AccountName)
            .Select(g => (Contributor: g.Key, Total: g.Sum(t => t.Amount)))
            .OrderByDescending(x => x.Total)
            .Take(3)
            .ToList();

        return topContributors;
    }

    /// <summary>
    /// Find most common receiver message patterns
    /// Useful for understanding expense categories
    /// </summary>
    public Dictionary<string, int> GetMostCommonMessages(List<Transaction> transactions, int topCount = 1)
{
    if (transactions == null || transactions.Count == 0)
        return new Dictionary<string, int>();

    var messageCounts = transactions
        .Where(t => !string.IsNullOrWhiteSpace(t.Message))
        .GroupBy(t => t.Message.Trim())
        .Select(g => new { Message = g.Key, Count = g.Count() })
        .OrderByDescending(x => x.Count)
        .Take(topCount)
        .ToDictionary(x => x.Message, x => x.Count);

    return messageCounts;
}
    /// <summary>
    /// Calculate statistics grouped by receiver (expense category)
    /// </summary>
    public Dictionary<string, (int Count, decimal Total)> GetStatisticsByCategory(List<Transaction> transactions)
    {
        if (transactions == null || transactions.Count == 0)
            return new Dictionary<string, (int, decimal)>();

        var statistics = transactions
            .GroupBy(t => t.AccountName)
            .Select(g => new
            {
                Category = g.Key,
                Count = g.Count(),
                Total = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => Math.Abs(x.Total))
            .ToDictionary(
                x => x.Category,
                x => (x.Count, x.Total)
            );

        return statistics;
    }
}