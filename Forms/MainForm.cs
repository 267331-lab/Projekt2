namespace NGOFinanceDashboard.Forms;
using NGOFinanceDashboard.Services;
using NGOFinanceDashboard.Utilities.Exceptions;
using NGOFinanceDashboard.Utilities;
using NGOFinanceDashboard.ViewModels;
using NGOFinanceDashboard.Models;

public partial class MainForm : Form
{
    private readonly MainFormViewModel _viewModel;
    private bool _isLoading = false;

    public MainForm()
    {
        InitializeComponent();

        IDataFetcher dataFetcher = CreateDataFetcher();
        IDataAnalyzer dataAnalyzer = new FinanceAnalyzer();

        _viewModel = new MainFormViewModel(dataFetcher, dataAnalyzer);

        // Subscribe na events
        _viewModel.StatusChanged += (s, e) => UpdateProgress(e.Message, e.Color);
        _viewModel.DataAnalyzed += (s, e) => DisplayAnalysis();

        this.fetchButton.Click += FetchButton_Click;
    }

    private static readonly Lazy<HttpClient> _httpClientLazy =
    new Lazy<HttpClient>(() => new HttpClient());

    private IDataFetcher CreateDataFetcher()
    {
        IFioParser parser = new FioHtmlParser();
        return new FioDataFetcher(_httpClientLazy.Value, parser);
    }
    private CancellationTokenSource? _cts;
    private async void FetchButton_Click(object? sender, EventArgs e)
{
    
    if (_isLoading) return;
    _cts = new CancellationTokenSource();  
    _isLoading = true;
    try {
        await _viewModel.FetchAndAnalyzeAsync(this.urlTextBox.Text, _cts.Token);
    }
    catch (DashboardException ex)
    {
        ExceptionHandler.HandleException(ex, "MainForm.FetchButton_Click");
        MessageBox.Show(ex.UserFriendlyMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    catch (Exception ex)
    {
        ExceptionHandler.HandleException(ex, "MainForm.FetchButton_Click");
        MessageBox.Show("Unknown error. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        _isLoading = false;
    }
}

    private void DisplayAnalysis()
    {
        try
        {
            FinanceReport financeReport = _viewModel.GetAnalysis();

            this.cashFlowValueLabel.Text = $"Total: {financeReport.CashFlow}";
            this.biggestExpenseValueLabel.Text = financeReport.BiggestExpense != null
                ? $"{financeReport.BiggestExpense.Amount}\n{financeReport.BiggestExpense.AccountName}"
                : "No expenses";
            this.topContributorsValueLabel.Text = financeReport.TopContributors.Count > 0
                ? string.Join("\n", financeReport.TopContributors.Select((c, i) => $"{i + 1}. {c.Item1}: {c.Item2}"))
                : "No contributors";
            this.commonMessagesValueLabel.Text = financeReport.Messages.Count > 0
                ? string.Join("\n\n", financeReport.Messages.Take(3).Select(m => $"{m.Key}: {m.Value}x"))
                : "No messages";

            DisplayTransactionGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Analysis error: {ex.Message}");
        }
    }

    private void DisplayTransactionGrid()
    {
        this.transactionsGrid.Rows.Clear();

        foreach (var transaction in _viewModel.GetTransactions())
        {
            var rowIndex = this.transactionsGrid.Rows.Add(
                transaction.Date.ToString("yyyy-MM-dd"),
                transaction.AccountName,
                transaction.Amount.ToString(),
                transaction.Message
            );

            var amountColor = transaction.IsIncome ? Color.Green : Color.Red;
            this.transactionsGrid.Rows[rowIndex].Cells["Amount"].Style.ForeColor = amountColor;
        }
    }

    private void UpdateProgress(string message, Color color)
    {
        this.progressLabel.Text = message;
        this.progressLabel.ForeColor = color;
    }

}