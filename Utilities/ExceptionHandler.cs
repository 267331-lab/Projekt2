using System.Diagnostics;

namespace NGOFinanceDashboard.Utilities;

/// <summary>
/// Centralized error handling for the application
/// </summary>
public static class ExceptionHandler
{
    private static readonly string LogFilePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, 
        "logs",
        "error.log");

    static ExceptionHandler()
    {
        // Vytvoř logs folder pokud neexistuje
        var logDir = Path.GetDirectoryName(LogFilePath);
        if (!Directory.Exists(logDir))
            Directory.CreateDirectory(logDir);
    }

    public static void HandleException(Exception ex, string context = "")
    {
        var exceptionType = ex.GetType().Name;
        var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] [{exceptionType}] {ex.Message}\n";
        
        if (ex.StackTrace != null)
            message += $"StackTrace: {ex.StackTrace}\n";
        
        if (ex.InnerException != null)
            message += $"InnerException: {ex.InnerException.Message}\n";
        
        message += new string('-', 80) + "\n";
        
        Debug.WriteLine(message);
        WriteToFile(message);
    }

    public static void LogInfo(string message, string context = "")
    {
        var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] INFO: {message}\n";
        Debug.WriteLine(logMessage);
        WriteToFile(logMessage);
    }

    public static void LogWarning(string message, string context = "")
    {
        var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] WARNING: {message}\n";
        Debug.WriteLine(logMessage);
        WriteToFile(logMessage);
    }

    private static void WriteToFile(string message)
    {
        try
        {
            File.AppendAllTextAsync(LogFilePath, message);
        }
        catch
        {
            // Silently fail if logging fails - nepřeruší aplikaci
            Debug.WriteLine("Failed to write to log file");
        }
    }
}