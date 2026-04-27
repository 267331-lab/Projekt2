namespace NGOFinanceDashboard.Models;

/// <summary>
/// Represents a financial transaction
/// </summary>
public class Transaction
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Counterparty { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Income,
    Expense,
    Transfer
}