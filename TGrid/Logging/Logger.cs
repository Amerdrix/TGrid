namespace Amerdrix.TGrid.Logging
{
    using System;

    class Logger : ILogger
    {
        public void Info(string message, params object[] obj)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message, obj);
        }

        public void Warning(string message, params object[] obj)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message, obj);
        }

        public void Error(string message, params object[] obj)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, obj);
        }
    }
}