namespace NGOFinanceDashboard.Forms;

using NGOFinanceDashboard.Models;
using NGOFinanceDashboard.Services;

public partial class MainForm : Form
{
    private readonly FioDataFetcher _dataFetcher;
    private readonly FinanceAnalyzer _dataAnalyzer;
    private List<Transaction> _currentTransactions = new();
    private bool _isLoading = false;

    public MainForm()
    {
        InitializeComponent();
        
        // Initialize services with required dependencies
        var httpClient = new HttpClient();
        var parser = new FioHtmlParser();  // You need to have this class
        _dataFetcher = new FioDataFetcher(httpClient, parser);
        _dataAnalyzer = new FinanceAnalyzer();
        
        // Wire up event handler
        this.fetchButton.Click += FetchButton_Click;
    }

    private async void FetchButton_Click(object? sender, EventArgs e)
    {
        await FetchDataAsync(this.urlTextBox.Text);
    }

    private async Task FetchDataAsync(string url)
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;
            System.Diagnostics.Debug.WriteLine("[FORM] [A] Calling FetchTransactionsAsync...");
            UpdateProgress("Loading data...", Color.Blue);

            // Pass the URL to FetchTransactionsAsync
            _currentTransactions = (await _dataFetcher.FetchTransactionsAsync(url)).ToList();

            System.Diagnostics.Debug.WriteLine("[FORM] [B] Got transactions!");
            AnalyzeAndDisplay();
            UpdateProgress($"✓ Loaded {_currentTransactions.Count} transactions", Color.Green);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FORM ERROR] {ex.GetType().FullName}: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"[FORM STACK] {ex.StackTrace}");
            UpdateProgress($"✗ Error: {ex.Message}", Color.Red);
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void AnalyzeAndDisplay()
    {
        try
        {
            var cashFlow = _dataAnalyzer.CalculateCashFlow(_currentTransactions);
            var biggestExpense = _dataAnalyzer.FindBiggestExpense(_currentTransactions);
            var topContributors = _dataAnalyzer.GetTop3Contributors(_currentTransactions);
            var commonMessages = _dataAnalyzer.GetMostCommonMessages(_currentTransactions);

            this.cashFlowValueLabel.Text = $"Total: {cashFlow}";
            
            this.biggestExpenseValueLabel.Text = biggestExpense != null
                ? $"{biggestExpense.Amount}\n{biggestExpense.AccountName}"
                : "No expenses";

            this.topContributorsValueLabel.Text = topContributors.Count > 0
                ? string.Join("\n", topContributors.Select((c, i) => $"{i + 1}. {c.Contributor}: {c.Total}"))
                : "No contributors";

            this.commonMessagesValueLabel.Text = commonMessages.Count > 0
                ? string.Join("\n\n", commonMessages.Take(3).Select(m => $"{m.Key}: {m.Value}x"))
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

        foreach (var transaction in _currentTransactions)
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