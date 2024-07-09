namespace LoggerTests;

using Logger;

[TestFixture]
public class NativeLoggerTests
{
    private ConsoleColor originalColor;
    private StringWriter consoleOutput;
    private NativeLogger logger;

    [SetUp]
    public void SetUp()
    {
        originalColor = Console.ForegroundColor;
        consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        logger = new NativeLogger(ConsoleColor.Blue);
    }

    [TearDown]
    public void TearDown()
    {
        Console.ForegroundColor = originalColor;
        Console.SetOut(Console.Out);
        consoleOutput.Dispose();
    }

    [Test]
    public void Log_WithMessage_ChangesConsoleColorAndWritesMessage()
    {
        logger.Log("Test message");
        
        StringAssert.Contains("Test message", consoleOutput.ToString());
        Assert.That(Console.ForegroundColor, Is.EqualTo(ConsoleColor.Blue));
    }

    [Test]
    public void LogWarning_WithMessage_ChangesConsoleColorToYellowAndWritesMessage()
    {
        logger.LogWarning("Warning message");

        StringAssert.Contains("Warning message", consoleOutput.ToString());
        Assert.That(Console.ForegroundColor, Is.EqualTo(ConsoleColor.Blue));
    }

    [Test]
    public void LogError_WithMessage_ChangesConsoleColorToRedAndWritesMessage()
    {
        logger.LogError("Error message");

        StringAssert.Contains("Error message", consoleOutput.ToString());
        Assert.That(Console.ForegroundColor, Is.EqualTo(ConsoleColor.Blue));
    }
}