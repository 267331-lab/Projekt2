using System.Diagnostics;

namespace NGOFinanceDashboard.Utilities;

/// <summary>
/// Centralized error handling for the application
/// </summary>
public static class ExceptionHandler
{
    private static readonly string LogFilePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, 
        "error.log");

    public static void HandleException(Exception ex, string context = "")
    {
        var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] {ex.Message}\n{ex.StackTrace}\n";
        
        Debug.WriteLine(message);
        
        try
        {
            File.AppendAllText(LogFilePath, message);
        }
        catch
        {
            // Silently fail if logging fails
        }
    }

    public static void LogInfo(string message, string context = "")
    {
        var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] INFO: {message}\n";
        Debug.WriteLine(logMessage);
    }
}