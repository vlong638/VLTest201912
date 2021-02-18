using System;
using System.IO;

namespace Autobots.Infrastracture.Common.LoggerSolution
{
    public class VLLogger
    {
        public VLLogger(string directory)
        {
            Directory = directory;
        }

        public string Directory { set; get; }

        public void Log(string text, string file = "log.txt")
        {
            var path = Path.Combine(Directory, file);
            File.AppendAllText(path, text);
        }
    }
    public interface ILogger
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Info(string message, Exception exception = null);
        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Warn(string message, Exception exception = null);
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Error(string message, Exception exception = null);
    }
}
