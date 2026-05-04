namespace NGOFinanceDashboard.ViewModels;

using NGOFinanceDashboard.Models;
using NGOFinanceDashboard.Services;
using NGOFinanceDashboard.Utilities.Exceptions;
using NGOFinanceDashboard.Utilities;
public class MainFormViewModel
{
    private readonly IDataFetcher _dataFetcher;
    private readonly IDataAnalyzer _dataAnalyzer;
    private List<Transaction> _currentTransactions = new();
    public event EventHandler<(string Message, Color Color)>? StatusChanged;
    public event EventHandler? DataAnalyzed;

    public MainFormViewModel(IDataFetcher dataFetcher, IDataAnalyzer dataAnalyzer)
    {
        _dataFetcher = dataFetcher;
        _dataAnalyzer = dataAnalyzer;
    }

    public async Task FetchAndAnalyzeAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            UpdateStatus("Loading data...", Color.Blue);
            _currentTransactions = (await _dataFetcher.FetchTransactionsAsync(url, cancellationToken)).ToList();
            UpdateStatus($"✓ Loaded {_currentTransactions.Count} transactions", Color.Green);
            DataAnalyzed?.Invoke(this, EventArgs.Empty);
        }
        catch (ValidationException ex)
        {
            ExceptionHandler.HandleException(ex, "MainFormViewModel.FetchAndAnalyzeAsync");
            UpdateStatus($"⚠️ {ex.UserFriendlyMessage}", Color.Orange);
        }
        catch (DataFetchException ex)
        {
            ExceptionHandler.HandleException(ex, "MainFormViewModel.FetchAndAnalyzeAsync");
            UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
        }
        catch (HtmlParsingException ex)
        {
            ExceptionHandler.HandleException(ex, "MainFormViewModel.FetchAndAnalyzeAsync");
            UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
        }
        catch (DashboardException ex)
        {
            ExceptionHandler.HandleException(ex, "MainFormViewModel.FetchAndAnalyzeAsync");
            UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
        }
        catch (OperationCanceledException)
        {
            UpdateStatus("⚠️ Načítání zrušeno", Color.Orange);
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex, "MainFormViewModel.FetchAndAnalyzeAsync");
            UpdateStatus("❌ Unknown error", Color.Red);
        }
    }


    public FinanceReport GetAnalysis()
    {
        FinanceReport financeReport = new();

        financeReport.CashFlow = _dataAnalyzer.CalculateCashFlow(_currentTransactions);
        financeReport.BiggestExpense = _dataAnalyzer.FindBiggestExpense(_currentTransactions);
        financeReport.TopContributors = _dataAnalyzer.GetTop3Contributors(_currentTransactions);
        financeReport.Messages = _dataAnalyzer.GetMostCommonMessages(_currentTransactions);

        return financeReport;
    }

    public List<Transaction> GetTransactions() => _currentTransactions;

    private void UpdateStatus(string message, Color color)
    {
        StatusChanged?.Invoke(this, (message, color));
    }
}