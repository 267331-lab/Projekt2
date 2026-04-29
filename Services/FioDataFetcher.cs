using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using HtmlDoc = HtmlAgilityPack.HtmlDocument;
using NGOFinanceDashboard.Models;
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
public class FioDataFetcher
{    
    private readonly HttpClient _httpClient;
    private readonly IFioParser _parser;
    private const string BaseUrl = "https://ib.fio.cz/ib/transparent?a=2200272480&f=27.04.2025&t=27.04.2026";

    public FioDataFetcher(HttpClient httpClient, IFioParser parser)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));

        _httpClient = new HttpClient();
        
        // Add comprehensive headers
        _httpClient.DefaultRequestHeaders.Add("User-Agent", 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        
        // Shorter timeout
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

   public async Task<IEnumerable<Transaction>> FetchTransactionsAsync(CancellationToken cancellationToken = default)
{
    Console.WriteLine("[1] Starting Selenium browser...");
    
    // Run synchronous Selenium code on a background thread
    return await Task.Run(() =>
    {
        Console.WriteLine("[2] Opening Fio Bank page...");
        
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        
        try
        {
            using (var driver = new ChromeDriver(options))
            {
                Console.WriteLine("[2] Navigating to URL...");
                driver.Navigate().GoToUrl("https://ib.fio.cz/ib/transparent?a=2200272480&f=27.04.2025&t=27.04.2026");
                
                Console.WriteLine("[3] Waiting for table to load...");
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElements(By.TagName("table")).Count > 0);
                
                Console.WriteLine("[4] Getting page source...");
                var pageSource = driver.PageSource;
                
                Console.WriteLine($"[5] Success! Got {pageSource.Length} characters");
                File.WriteAllText("fio_page.html", pageSource);
                
                IEnumerable<Transaction> parsedHTML = _parser.ParseRawHtml(pageSource);
                Console.WriteLine(parsedHTML);
                return parsedHTML;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            throw;
        }
    }, cancellationToken);
}
}

/// <summary>
/// Concrete implementation of the Fio HTML parser.
/// </summary>
public class FioHtmlParser : IFioParser
{
    public IEnumerable<Transaction> ParseRawHtml(string html)
    {
        var transactions = new List<Transaction>();
        var doc = new HtmlDoc();
        doc.LoadHtml(html);


        // Targeted selector for the transaction table rows
        var header = doc.DocumentNode.SelectNodes("//table[@class='table table-striped table-condensed']/thead/tr");
        var rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped table-condensed']/tbody/tr");

        if (rows == null) return transactions;

        foreach (var row in rows)
        {
            var cells = row.SelectNodes("td");
            if (cells == null || cells.Count < 8) continue;

            transactions.Add(new Transaction
            {
                Date = DateTime.ParseExact(cells[0].InnerText.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Amount = decimal.Parse(cells[1].InnerText.Replace("&nbsp;", "").Replace(" ", "").Trim(), CultureInfo.InvariantCulture),
                Currency = cells[1].InnerText.Trim(),
                Type = cells[2].InnerText.Trim(),
                AccountName = cells[3].InnerText.Trim(),
                Message = cells[4].InnerText.Trim(),
                ContactSymbol = cells[5].InnerText.Trim(),
                VariableSymbol = cells[6].InnerText.Trim(),
                SpecificSymbol = cells[7].InnerText.Trim(),
                Note = cells[8].InnerText.Trim(),
            });
        }

        return transactions;
    }
}