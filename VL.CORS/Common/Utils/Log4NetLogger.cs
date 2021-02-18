using Autobots.Infrastracture.Common.LoggerSolution;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// Log4Net Logger
    /// </summary>
    public class Log4NetLogger: ILogger
    {
        internal ILog logger;
        internal ILog sqlLogger;

        static Log4NetLogger _Log4NetLogger;

        static Log4NetLogger()
        {
            _Log4NetLogger = new Log4NetLogger();
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("configs/log4net.config"));
            _Log4NetLogger.logger = LogManager.GetLogger(repository.Name, "filelogger");
            XmlConfigurator.Configure(repository, new FileInfo("configs/log4net.config"));
            _Log4NetLogger.sqlLogger = LogManager.GetLogger(repository.Name, "sqlLogger");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Log4NetLogger GetLogger()
        {
            return _Log4NetLogger;
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(string message, Exception exception = null)
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
        public void Warn(string message, Exception exception = null)
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
        public void Error(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }

        /// <summary>
        /// DHF日志
        /// </summary>
        public void LogSQL(string sql, Dictionary<string, object> pars)
        {
            StringBuilder sb = new StringBuilder();
            if (pars != null)
            {
                foreach (var par in pars)
                {
                    sb.AppendLine($"declare @{par.Key} nvarchar(50);  set @{par.Key} = '{par.Value}' ");
                }
            }
            sb.Append(sql);
            sqlLogger.Error(sb.ToString());
        }
    }
}
