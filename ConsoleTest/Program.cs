using Logger;

var logger = new ConsoleLogger("Console");
logger.Log("Hello, World!");
logger.LogWarning("This is a warning!");
logger.LogError("This is an error!");