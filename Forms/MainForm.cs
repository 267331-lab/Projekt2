namespace NGOFinanceDashboard.Forms;
using NGOFinanceDashboard.Services;
using NGOFinanceDashboard.Utilities.Exceptions;
using NGOFinanceDashboard.Utilities;
using NGOFinanceDashboard.ViewModels;

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

    private async void FetchButton_Click(object? sender, EventArgs e)
{
    if (_isLoading) return;

    _isLoading = true;
    try
    {
        await _viewModel.FetchAndAnalyzeAsync(this.urlTextBox.Text);
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
            var (cashFlow, biggestExpense, topContributors, messages) = _viewModel.GetAnalysis();

            this.cashFlowValueLabel.Text = $"Total: {cashFlow}";
            this.biggestExpenseValueLabel.Text = biggestExpense != null
                ? $"{biggestExpense.Amount}\n{biggestExpense.AccountName}"
                : "No expenses";
            this.topContributorsValueLabel.Text = topContributors.Count > 0
                ? string.Join("\n", topContributors.Select((c, i) => $"{i + 1}. {c.Item1}: {c.Item2}"))
                : "No contributors";
            this.commonMessagesValueLabel.Text = messages.Count > 0
                ? string.Join("\n\n", messages.Take(3).Select(m => $"{m.Key}: {m.Value}x"))
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