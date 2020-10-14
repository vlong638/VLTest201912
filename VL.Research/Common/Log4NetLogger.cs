using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.Research.Common
{
    /// <summary>
    /// Log4Net Logger
    /// </summary>
    public class Log4NetLogger
    {
        private static ILog logger;
        private static ILog sqlLogger;

        static Log4NetLogger()
        {
            var repository = LogManager.CreateRepository("NETCoreRepository");
            if (logger == null)
            {
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("configs/log4net.config"));
                logger = LogManager.GetLogger(repository.Name, "filelogger");
            }
            if (sqlLogger == null)
            {
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("configs/log4net.config"));
                sqlLogger = LogManager.GetLogger(repository.Name, "sqlLogger");
            }
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Warn(message);
            else
                logger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }

        /// <summary>
        /// DHF日志
        /// </summary>
        public static void LogSQL(string sql, Dictionary<string, object> pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var par in pars)
            {
                sb.AppendLine($"declare @{par.Key} nvarchar(50)");
                sb.AppendLine($" set @{par.Key} = '{par.Value}'");
            }
            sb.Append(sql);
            sqlLogger.Info(sb.ToString());
        }
    }
}
