using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VLAutobots.Infrastracture.Common.FileSolution;

namespace Autobots.Infrastracture.LogCenter
{
    /// <summary>
    /// Log4Net Logger
    /// </summary>
    public class Log4NetLogger
    {
        private static ILog systemLogger;
        private static ILog fileLogger;
        private static ILog sqlLogger;

        static Log4NetLogger()
        {
            var repository = LogManager.CreateRepository("LoggerRepository");
            var log4netConfig = Path.Combine(FileHelper.GetDirectory("configs"), "log4net.config");
            XmlConfigurator.Configure(repository, new FileInfo(log4netConfig));
            if (systemLogger == null)
            {
                systemLogger = LogManager.GetLogger(repository.Name, "systemlogger");
            }
            if (fileLogger == null)
            {
                fileLogger = LogManager.GetLogger(repository.Name, "filelogger");
            }
            if (sqlLogger == null)
            {
                sqlLogger = LogManager.GetLogger(repository.Name, "sqllogger");
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void SystemError(Exception exception)
        {
            systemLogger.Error(exception);
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                fileLogger.Info(message);
            else
                fileLogger.Info(message, exception);
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                fileLogger.Warn(message);
            else
                fileLogger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception == null)
                fileLogger.Error(message);
            else
                fileLogger.Error(message, exception);
        }

        /// <summary>
        /// DHF日志
        /// </summary>
        public static void LogSQL(string sql, Dictionary<string, object> pars)
        {
            StringBuilder sb = new StringBuilder();
            if (pars != null)
            {
                foreach (var par in pars)
                {
                    sb.AppendLine($"declare @{par.Key} nvarchar(50)");
                    sb.AppendLine($" set @{par.Key} = '{par.Value}'");
                }
            }
            sb.Append(sql);
            sqlLogger.Error(sb.ToString());
        }
    }
}