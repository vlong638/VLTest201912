using System;
using System.Collections.Generic;
using VL.Consolo_Core.Common.FileSolution;

namespace BBee.Common.Configuration
{
    /// <summary>
    /// 配置样例
    /// </summary>
    public class DHFConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsLogSQL { set; get; }

        internal void DoLog(string sql, Dictionary<string, object> pars)
        {
            if (IsLogSQL)
            {
                Log4NetLogger.LogSQL(sql, pars);
            }
        }
    }
}


