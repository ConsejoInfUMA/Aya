using System;

namespace Aya.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string msg)
        {
            Console.WriteLine($"ERROR: {msg}");
        }

        public void LogInfo(string msg)
        {
            Console.WriteLine($"INFO: {msg}");
        }
    }
}

