namespace Logger;

[Serializable]
public class ConsoleLogger(string source = "Console") : ILogger
{
    private string source = source;

    public void Log(string message)
    {
        Log(message, Level.Log);
    }

    public void LogWarning(string message)
    {
        Log(message, Level.Warning);
    }

    public void LogError(string message)
    {
        Log(message, Level.Error);
    }
    
    private void Log(string message, Level level)
    {
        Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}][{source}][{level.ToString()}]: {message}");
    }
    
    private enum Level
    {
        Log,
        Warning,
        Error
    }
}