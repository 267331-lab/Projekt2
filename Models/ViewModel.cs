namespace NGOFinanceDashboard.ViewModels;

using NGOFinanceDashboard.Models;
using NGOFinanceDashboard.Services;
using NGOFinanceDashboard.Utilities.Exceptions;
public class MainFormViewModel
{
    private readonly IDataFetcher _dataFetcher;
    private readonly IDataAnalyzer _dataAnalyzer;
    private List<Transaction> _currentTransactions = new();

    public event EventHandler<string>? ProgressUpdated;
    public event EventHandler<(string Message, Color Color)>? StatusChanged;
    public event EventHandler? DataAnalyzed;

    public MainFormViewModel(IDataFetcher dataFetcher, IDataAnalyzer dataAnalyzer)
    {
        _dataFetcher = dataFetcher;
        _dataAnalyzer = dataAnalyzer;
    }

    public async Task FetchAndAnalyzeAsync(string url)
{
    try
    {
        UpdateStatus("Loading data...", Color.Blue);
        _currentTransactions = (await _dataFetcher.FetchTransactionsAsync(url)).ToList();
        UpdateStatus($"✓ Loaded {_currentTransactions.Count} transactions", Color.Green);
        DataAnalyzed?.Invoke(this, EventArgs.Empty);
    }
    catch (ValidationException ex)
    {
        UpdateStatus($"⚠️ {ex.UserFriendlyMessage}", Color.Orange);
        LogException(ex);
    }
    catch (DataFetchException ex)
    {
        UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
        LogException(ex);
    }
    catch (HtmlParsingException ex)
    {
        UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
        LogException(ex);
    }
    catch (DashboardException ex)
    {
        UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
        LogException(ex);
    }
    catch (Exception ex)
    {
        UpdateStatus("❌ Neznámá chyba. Kontaktuj support.", Color.Red);
        LogException(new DataAnalysisException("Unexpected error", ex));
    }
}

private void LogException(DashboardException ex)
{
    System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {ex.GetType().Name}: {ex.LogDetails}");
    // Sem by šel i file logging
}
    public (decimal CashFlow, Transaction? BiggestExpense, List<(string, decimal)> TopContributors, Dictionary<string, int> Messages) GetAnalysis()
    {
        var cashFlow = _dataAnalyzer.CalculateCashFlow(_currentTransactions);
        var biggestExpense = _dataAnalyzer.FindBiggestExpense(_currentTransactions);
        var topContributors = _dataAnalyzer.GetTop3Contributors(_currentTransactions);
        var messages = _dataAnalyzer.GetMostCommonMessages(_currentTransactions);

        return (cashFlow, biggestExpense, topContributors, messages);
    }

    public List<Transaction> GetTransactions() => _currentTransactions;

    private void UpdateStatus(string message, Color color)
    {
        StatusChanged?.Invoke(this, (message, color));
    }
}