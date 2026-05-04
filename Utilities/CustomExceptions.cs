namespace NGOFinanceDashboard.Utilities.Exceptions;

/// <summary>
/// Base exception pro NGO Dashboard
/// </summary>
public abstract class DashboardException : Exception
{
    public string UserFriendlyMessage { get; }
    public string LogDetails { get; }

    protected DashboardException(string message, string userFriendlyMessage, string? logDetails = null, Exception innerException = null) 
        : base(message, innerException)
    {
        UserFriendlyMessage = userFriendlyMessage;
        LogDetails = logDetails ?? message;
    }
}

/// <summary>
/// Problém při načítání dat z Fio
/// </summary>
public class DataFetchException : DashboardException
{
    public string? Url { get; }
    public int? HttpStatusCode { get; }

    public DataFetchException(string url, string message, Exception? inner, int? statusCode = null)
        : base(
        $"Failed to fetch from {url}: {message}",
        "Nepodařilo se načíst data...",
        inner?.ToString(),
        inner
        )
    {
        Url = url;
        HttpStatusCode = statusCode;
    }
}

/// <summary>
/// Problém při parsování HTML
/// </summary>
public class HtmlParsingException : DashboardException
{
    public HtmlParsingException(string message, Exception innerException)
        : base(
            $"Failed to parse HTML: {message}",
            "Formulář Fio banky se změnil. Kontaktuj administrátora.",
            message,
            innerException
        )
    {
    }
}

/// <summary>
/// Problém při analýze dat
/// </summary>
public class DataAnalysisException : DashboardException
{
    public DataAnalysisException(string message, Exception? innerException = null)
        : base(
            $"Analysis failed: {message}",
            "Chyba při analýze dat. Zkus to znovu.",
            innerException?.ToString()
        )
    {
    }
}

/// <summary>
/// Validační chyba
/// </summary>
public class ValidationException : DashboardException
{
    public ValidationException(string fieldName, string message)
        : base(
            $"Validation failed for {fieldName}: {message}",
            $"Neplatný vstup: {message}",
            null
        )
    {
    }
}

public class InvalidUrlException : DashboardException
{
    public InvalidUrlException(string message)
    : base(
            $"Is not a Fio transparent account url: {message}",
            $"Neplatný vstup: {message}",
            null
        ){}
}