using Autobots.EMRServices.FileSolution;
using log4net;
using log4net.Config;
using System;
using System.IO;

namespace Autobots.CommonServices.Utils
{
    /// <summary>
    /// Log4Net Logger
    /// </summary>
    public class Log4NetLogger
    {
        private ILog systemLogger;

        public Log4NetLogger()
        {
            var repository = LogManager.CreateRepository("LoggerRepository");
            var log4netConfig = Path.Combine(FileHelper.GetDirectoryToOutput("configs"), "log4net.config");
            XmlConfigurator.Configure(repository, new FileInfo(log4netConfig));
            if (systemLogger == null)
            {
                systemLogger = LogManager.GetLogger(repository.Name, "systemlogger");
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            systemLogger.Error(exception.ToString());
        }
    }
}