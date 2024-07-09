namespace Logger;

public class NativeLogger : ILogger
{
    private ConsoleColor defaultColor;
    
    public NativeLogger(ConsoleColor defaultColor)
    {
        this.defaultColor = defaultColor;
        Console.ForegroundColor = defaultColor;
    }
    
    public void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        Console.ForegroundColor = defaultColor;
    }

    public void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ForegroundColor = defaultColor;
    }

    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = defaultColor;
    }
}