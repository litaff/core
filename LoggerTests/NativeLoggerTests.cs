namespace LoggerTests;

using Logger;

[TestFixture]
public class NativeLoggerTests
{
    private StringWriter consoleOutput;
    private NativeLogger logger;

    [SetUp]
    public void SetUp()
    {
        consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        logger = new NativeLogger();
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(Console.Out);
        consoleOutput.Dispose();
    }

    [Test]
    public void Log_WithMessage_WritesExpectedFormattedMessage()
    {
        logger.Log("Test log");
        StringAssert.Contains("[NativeLogger] Log: Test log", consoleOutput.ToString());
    }

    [Test]
    public void LogWarning_WithMessage_WritesExpectedFormattedMessage()
    {
        logger.LogWarning("Test warning");
        StringAssert.Contains("[NativeLogger] Warning: Test warning", consoleOutput.ToString());
    }

    [Test]
    public void LogError_WithMessage_WritesExpectedFormattedMessage()
    {
        logger.LogError("Test error");
        StringAssert.Contains("[NativeLogger] Error: Test error", consoleOutput.ToString());
    }

    [Test]
    public void Log_WithNullMessage_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => logger.Log(null!));
    }

    [Test]
    public void LogWarning_WithNullMessage_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => logger.LogWarning(null!));
    }

    [Test]
    public void LogError_WithNullMessage_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => logger.LogError(null!));
    }
    
    [Test]
    public void Log_WithEmptyMessage_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => logger.Log(string.Empty));
    }
    
    [Test]
    public void LogWarning_WithEmptyMessage_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => logger.LogWarning(string.Empty));
    }
    
    [Test]
    public void LogError_WithEmptyMessage_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => logger.LogError(string.Empty));
    }
}