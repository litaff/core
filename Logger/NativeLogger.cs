namespace Logger;

[Serializable]
public class NativeLogger : ILogger
{
    public void Log(string message)
    {
        if(string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message));
        }
        Console.WriteLine($"[NativeLogger] Log: {message}");
    }

    public void LogWarning(string message)
    {
        if(string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message));
        }
        Console.WriteLine($"[NativeLogger] Warning: {message}");
    }

    public void LogError(string message)
    {
        if(string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message));
        }
        Console.WriteLine($"[NativeLogger] Error: {message}");
    }
}