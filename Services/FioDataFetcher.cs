
using Microsoft.Playwright;
using HtmlDoc = HtmlAgilityPack.HtmlDocument;
using NGOFinanceDashboard.Models;
using NGOFinanceDashboard.Utilities.Exceptions;
using NGOFinanceDashboard.Utilities;

using System.Globalization;

namespace NGOFinanceDashboard.Services;

/// <summary>
/// Defines the contract for parsing Fio Bank's HTML structure.
/// </summary>
public interface IFioParser
{
    IEnumerable<Transaction> ParseRawHtml(string html);
}

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
        string fioAccountUrl = "", CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fioAccountUrl))
            throw new ValidationException(nameof(fioAccountUrl), "URL nemůže být prázdné");
        if (!fioAccountUrl.Contains("ib.fio.cz/ib/transparent?"))
            throw new InvalidURLException($"Zadaná Url {fioAccountUrl} nepatří FIO transparentnímu účtu");

        try
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GotoAsync(fioAccountUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
            await page.WaitForSelectorAsync("//table[contains(@class,'table')]/tbody/tr");

            var pageSource = await page.ContentAsync();
            return _parser.ParseRawHtml(pageSource);
        }
        catch (PlaywrightException ex)
        {
            ExceptionHandler.HandleException(ex, "FioDataFetcher.FetchTransactionsAsync");
            throw new DataFetchException(fioAccountUrl, "Playwright failed", null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex, "FioDataFetcher.FetchTransactionsAsync");
            throw new DataFetchException(fioAccountUrl, ex.Message, null);
        }
    }
}

/// <summary>
/// Concrete implementation of the Fio HTML parser.
/// </summary>
public class FioHtmlParser : IFioParser
{
    public IEnumerable<Transaction> ParseRawHtml(string html)
    {
        try
        {
            var transactions = new List<Transaction>();
            var doc = new HtmlDoc();
            doc.LoadHtml(html);

            var rows = doc.DocumentNode.SelectNodes("//table[contains(@class,'table')]/tbody/tr");

            if (rows == null || rows.Count == 0)
                throw new HtmlParsingException("Tabulka s transakcemi nenalezena");

            foreach (var row in rows)
            {
                try
                {
                    var cells = row.SelectNodes("td");
                    if (cells == null || cells.Count < 8) continue;

                    transactions.Add(new Transaction
                    {
                        Date = DateTime.ParseExact(cells[0].InnerText.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Amount = decimal.Parse(cells[1].InnerText.Replace("&nbsp;", "").Replace("CZK", "").Replace(" ", "").Replace(",", ".").Trim(), CultureInfo.InvariantCulture),
                        Currency = cells[1].InnerText.Trim(),
                        Type = cells[2].InnerText.Trim(),
                        AccountName = cells[3].InnerText.Trim(),
                        Message = cells[4].InnerText.Trim(),
                        ContactSymbol = cells[5].InnerText.Trim(),
                        VariableSymbol = cells[6].InnerText.Trim(),
                        SpecificSymbol = cells[7].InnerText.Trim(),
                        Note = cells.Count > 8 ? cells[8].InnerText.Trim() : "",
                    });
                }
                catch (FormatException ex)
                {
                    ExceptionHandler.HandleException(ex, "FioHtmlParser.ParseRawHtml");
                    throw new HtmlParsingException($"Format error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "FioHtmlParser.ParseRawHtml");
                    throw new HtmlParsingException(ex.Message);
                }
            }

            return transactions;
        }
        catch (DashboardException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new HtmlParsingException(ex.Message);
        }
    }
}