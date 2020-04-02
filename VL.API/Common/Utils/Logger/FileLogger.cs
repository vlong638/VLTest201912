using System;
using System.IO;

namespace VL.API.Common.Utils
{
    public class FileLogger : ILogger
    {
        static string _Path;
        static string Path
        {
            get
            {
                if (string.IsNullOrEmpty(_Path))
                {
                    _Path = @"D:\log.txt";
                }
                return _Path;
            }
        }

        public void Info(LogData locator)
        {
            File.AppendAllText(Path, locator.ToString());
        }

        public void Info(string message)
        {
            Info(new LogData(message));
        }

        public void Info(Exception ex)
        {
            Info(new LogData(ex.ToString()));
        }

        public void Error(LogData locator)
        {
            File.AppendAllText(Path, locator.ToString());
        }

        public void Error(string message)
        {
            Error(new LogData(message));
        }

        public void Error(Exception ex)
        {
            Error(new LogData(ex.ToString()));
        }
    }
}
