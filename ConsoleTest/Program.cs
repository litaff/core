using Logger;

var logger = new NativeLogger(ConsoleColor.White);
logger.Log("Hello, World!");
logger.LogWarning("This is a warning!");
logger.LogError("This is an error!");