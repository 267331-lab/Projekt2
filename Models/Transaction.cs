namespace NGOFinanceDashboard.Models;

public class Transaction
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Type {get;set;}
    public string AccountName { get; set; }
    public string Message { get; set; }
    public string ContactSymbol {get; set;}
    public string VariableSymbol {get; set;}
    public string SpecificSymbol {get; set;}
    public string Note {get; set;}
    public bool IsIncome => Amount > 0;
    public bool IsExpense => Amount < 0;

}