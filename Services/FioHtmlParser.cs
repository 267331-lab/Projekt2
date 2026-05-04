
using NGOFinanceDashboard.Models;
using HtmlDoc = HtmlAgilityPack.HtmlDocument;
using NGOFinanceDashboard.Utilities.Exceptions;
using NGOFinanceDashboard.Utilities;
using System.Globalization;

namespace NGOFinanceDashboard.Services;
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
                throw new HtmlParsingException("Tabulka s transakcemi nenalezena", null);

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
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "FioHtmlParser.ParseRawHtml");
                    throw new HtmlParsingException($"Could not parse Fio bank data: {ex.Message}", ex);
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
            throw new HtmlParsingException(ex.Message, ex);
        }
    }
}