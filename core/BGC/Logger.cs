using System;
using System.Collections.Generic;
using System.Text;

namespace BGC
{
    class Logger
    {
        public enum LoggingLevels
        {
            Debug = 3,
            HighLogging = 2,
            MinimalLogging = 1,
            NoLogging = 0
        }

        public static LoggingLevels LoggingLevel = LoggingLevels.Debug;

        public static void Log(string message, LoggingLevels level)
        {
            if (level <= LoggingLevel)
            {
                // Log it however you want
                Console.Out.WriteLine(message);
            }
        }
    }
}
