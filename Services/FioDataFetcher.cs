
using Microsoft.Playwright;
using NGOFinanceDashboard.Models;
using NGOFinanceDashboard.Utilities.Exceptions;
using NGOFinanceDashboard.Utilities;
namespace NGOFinanceDashboard.Services;

/// <summary>
/// Fetches transaction data from Fio Bank's transparent account page.
/// </summary>
public class FioDataFetcher : IDataFetcher
{
    private readonly HttpClient _httpClient;
    private readonly IFioParser _parser;
    public FioDataFetcher(HttpClient httpClient, IFioParser parser)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<IEnumerable<Transaction>> FetchTransactionsAsync(
        string fioAccountUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fioAccountUrl))
            throw new ValidationException(nameof(fioAccountUrl), "URL nemůže být prázdné");
        if (!Uri.TryCreate(fioAccountUrl, UriKind.Absolute, out var uri) ||
    !uri.Host.EndsWith("fio.cz") ||
    !uri.AbsolutePath.Contains("/ib/transparent") ||
    !uri.Query.Contains("a="))
            throw new InvalidUrlException($"Zadaná Url {fioAccountUrl} nepatří FIO transparentnímu účtu");

        try
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            
            cancellationToken.ThrowIfCancellationRequested();
            await page.GotoAsync(fioAccountUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 5000});
            cancellationToken.ThrowIfCancellationRequested();

            await page.WaitForSelectorAsync("//table[contains(@class,'table')]/tbody/tr");

            var pageSource = await page.ContentAsync();
            return _parser.ParseRawHtml(pageSource);
        }
        catch (PlaywrightException ex)
        {
            ExceptionHandler.HandleException(ex, "FioDataFetcher.FetchTransactionsAsync");
            throw new DataFetchException(fioAccountUrl, "Playwright failed",ex);
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex, "FioDataFetcher.FetchTransactionsAsync");
            throw new DataFetchException(fioAccountUrl, ex.Message, ex);
        }
    }
}

