using Microsoft.Extensions.Options;
using System;
using System.Data;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Research.Common.Configuration;

namespace VL.Research.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class APIContext:DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public APIContext(IOptions<DBConfig> loggingConfig) : base()
        {
            var connectingStr = DBHelper.GetDbConnection(loggingConfig.Value.ConnectionString);
            DbGroup = new DbGroup(connectingStr);
        }
    }
}
